namespace Andreas.PowerGrip.Server.Services;

public class AppUserManager(
    ILogger<AppUserManager> logger,
    IJwtProvider jwtProvider,
    ISystemService system,
    AppDbContext context) : IAppUserManager
{
    private readonly ILogger<AppUserManager> _logger = logger;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly ISystemService _system = system;
    private readonly AppDbContext _context = context;

    private bool SaveChangesToDatabase()
    {
        _logger.LogDebug("Saving changes...");
        try
        {
            int updated = _context.SaveChanges();
            if (updated > 0)
            {
                _logger.LogInformation("Changes saved!");
                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save changes.");
        }
        return false;
    }

    private async Task<bool> SaveChangesToDatabaseAsync()
    {
        _logger.LogDebug("Saving changes...");
        try
        {
            int updated = await _context.SaveChangesAsync();
            if (updated > 0)
            {
                _logger.LogInformation("Changes saved!");
                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save changes.");
        }
        return false;
    }

    public AppUser? GetUserById(Guid id)
        => GetUserByIdAsync(id).GetAwaiter().GetResult();

    public async Task<AppUser?> GetUserByIdAsync(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        return user;
    }

    public AppUser? GetUserByUsername(string username)
        => GetUserByUsernameAsync(username).GetAwaiter().GetResult();

    public async Task<AppUser?> GetUserByUsernameAsync(string username)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
        return user;
    }

    public AppUser? GetUserByEmail(string email)
        => GetUserByEmailAsync(email).GetAwaiter().GetResult();

    public async Task<AppUser?> GetUserByEmailAsync(string email)
    {
        var normalizedEmail = email.ToLowerInvariant().Trim();
        var user = await _context.Users.FirstOrDefaultAsync(u => u.EmailAddress == normalizedEmail);
        return user;
    }

    public bool CheckPassword(AppUser user, string password)
    {
        return HashUtils.ComparePasswords(password, user.PasswordHash, user.PasswordSalt);
    }

    public AuthResponse Login(AppUser user)
        => LoginAsync(user).GetAwaiter().GetResult();

    public async Task<AuthResponse> LoginAsync(AppUser user)
    {
        // Let the controllers handle checking if the user exists.
        // if (GetUserById(user.Id) is not null)
        // {
        //     return AuthResponse.FailedLoginAttempt("Login Failed", "Invalid Credentials");
        // }

        List<Claim> claims = user.GetBaseClaims();

        claims.AddRange(
            user.Roles.Select(
                role => new Claim(ClaimTypes.Role, role)));

        var tokens = await _jwtProvider.GenerateTokensAsync(claims);

        await _context.RefreshTokens.AddAsync(new RefreshToken
        {
            UserId = user.Id,
            Revoked = false,
            Token = tokens.RefreshToken,
            ExpiresAt = DateTime.UtcNow.Add(tokens.RefreshLifetime),
        });

        bool updated = await SaveChangesToDatabaseAsync();

        return AuthResponse.ForLoginStatus(updated, tokens);
    }

    public ServiceResponse Logout(AppUser user, PgLogoutRequest request)
        => LogoutAsync(user, request).GetAwaiter().GetResult();

    public async Task<ServiceResponse> LogoutAsync(AppUser user, PgLogoutRequest request)
    {
        if (string.IsNullOrEmpty(request.RefreshToken))
        {
            // Global Logout
            var allTokens = _context.RefreshTokens.Where(t => t.UserId == user.Id);
            _context.RefreshTokens.RemoveRange(allTokens);

            if (await SaveChangesToDatabaseAsync())
            {
                return ServiceResponse.SuccessResponse("Logged out from all devices.");
            }
            else
            {
                return ServiceResponse.ErrorResponse("Logout failed.", ServiceError.InternalServerError("Could not remove tokens"));
            }
        }
        else
        {
            // Logout from specific device
            var token = await _context.RefreshTokens.SingleOrDefaultAsync(t => t.UserId == user.Id && t.Token == request.RefreshToken);

            if (token == null)
            {
                return ServiceResponse.ErrorResponse("Invalid token.", ServiceError.NotFound("refresh token"));
            }

            _context.RefreshTokens.Remove(token);

            if (await SaveChangesToDatabaseAsync())
            {
                return ServiceResponse.SuccessResponse("Logged out from this device.");
            }
            else
            {
                return ServiceResponse.ErrorResponse("Logout failed.", ServiceError.InternalServerError("Could not remove token"));
            }
        }
    }

    public AuthResponse RefreshSession(RefreshRequest request)
        => RefreshSessionAsync(request).GetAwaiter().GetResult();

    public async Task<AuthResponse> RefreshSessionAsync(RefreshRequest request)
    {
        var user = await _context.Users
            .Include(u => u.RefreshTokens)
            .SingleOrDefaultAsync(u => u.RefreshTokens.Any(r => r.Token == request.Refresh));

        if (user is null)
        {
            return AuthResponse.ErrorResponse("Cannot Refresh", ServiceError.SessionExpired());
        }

        var refreshToken = user.RefreshTokens.SingleOrDefault(r => r.Token == request.Refresh);

        if (refreshToken == null || refreshToken.IsExpired)
        {
            return AuthResponse.ErrorResponse("Refresh token expired or invalid", ServiceError.SessionExpired());
        }

        List<Claim> claims = user.GetBaseClaims();

        claims.AddRange(
            user.Roles.Select(
                role => new Claim(ClaimTypes.Role, role)));

        var tokens = _jwtProvider.GenerateTokens(claims);

        // Replace old refresh token with the new one
        refreshToken.Token = tokens.RefreshToken;
        refreshToken.ExpiresAt = DateTime.UtcNow.Add(tokens.RefreshLifetime);

        if (await SaveChangesToDatabaseAsync())
        {
            return AuthResponse.SuccessResponse("Refresh Succeeded", tokens);
        }

        return AuthResponse.ErrorResponse(
            title: "Refresh Failed",
            error: ServiceError.InternalServerError("Couldn't save refresh token")
        );
    }

    public List<string> GetRoles(AppUser user)
        => GetRolesAsync(user).GetAwaiter().GetResult();

    public async Task<List<string>> GetRolesAsync(AppUser user)
    {
        var result = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
        return result?.Roles ?? [];
    }

    public bool IsInRole(AppUser user, string role)
        => IsInRoleAsync(user, role).GetAwaiter().GetResult();

    public async Task<bool> IsInRoleAsync(AppUser user, string role)
    {
        return await _context.Users
            .Where(u => u.Id == user.Id)
            .SelectMany(u => u.Roles)
            .AnyAsync(r => r.SameAs(role));
    }

    private ServiceResponse ValidateUniqueness(ModifyUserRequest request)
        => ValidateUniquenessAsync(request).GetAwaiter().GetResult();

    private async Task<ServiceResponse> ValidateUniquenessAsync(ModifyUserRequest request)
    {
        var existingUser = await GetUserByUsernameAsync(request.Username);

        var response = ServiceResponse.SuccessResponse("User created successfully");

        if (existingUser is not null)
        {

            _logger.LogError("User with the name {username} already exists", existingUser.Username);

            response.Success = false;
            response.Title = "User creation failed";
            response.AddError(ServiceError.DuplicateUsername(existingUser.Username));
        }

        existingUser = await GetUserByEmailAsync(request.EmailAddress);

        if (existingUser is not null)
        {
            _logger.LogError("User with the email {emailAddress} already exists", existingUser.EmailAddress);
            response.Success = false;
            response.Title = "User creation failed";
            response.AddError(ServiceError.DuplicateEmail(existingUser.EmailAddress));
        }

        return response;
    }

    private ServiceResponse ValidateCorrectness(ModifyUserRequest request, bool allowEmpty = false)
    {
        var response = ServiceResponse.SuccessResponse("Request valid");
        bool emptyWhenNotAllowed;

        emptyWhenNotAllowed = !allowEmpty && request.Username.IsWhiteSpace();

        if (emptyWhenNotAllowed || !AppUser.IsValidUsername(request.Username))
        {
            response.Title = "Request failed";
            response.Success = false;
            response.AddError(ServiceError.InvalidData("Usernames can only contain letters, numbers, and symbols"));
        }

        emptyWhenNotAllowed = !allowEmpty && request.SystemUsername.IsWhiteSpace();
        if (emptyWhenNotAllowed || !_system.IsSystemUser(request.SystemUsername))
        {
            response.Title = "Request failed";
            response.Success = false;
            response.AddError(ServiceError.InvalidData("Each account must associate with a user on this system"));
        }

        emptyWhenNotAllowed = !allowEmpty && request.EmailAddress.IsWhiteSpace();
        if (emptyWhenNotAllowed || !CommonValidators.IsValidEmail(request.EmailAddress))
        {
            response.Title = "Request failed";
            response.Success = false;
            response.AddError(ServiceError.InvalidData("The email address must be a valid one"));
        }

        emptyWhenNotAllowed = !allowEmpty && request.BirthDate is null;
        if (emptyWhenNotAllowed || request.BirthDate! > DateTime.UtcNow)
        {
            response.Title = "Request failed";
            response.Success = false;
            response.AddError(ServiceError.InvalidData("Birth dates cannot be set in the future"));
        }

        if (request is CreateUserRequest createRequest)
        {
            emptyWhenNotAllowed = !allowEmpty && (createRequest.Roles is null || !createRequest.Roles.Any());
            if (emptyWhenNotAllowed && !createRequest.Roles!.All(AppRoles.IsKnown))
            {
                response.Title = "Request failed";
                response.Success = false;
                response.AddError(ServiceError.InvalidData("One or more roles given are incorrect"));
            }
        }

        return response;
    }

    public ServiceResponse<AppUser> CreateUser(CreateUserRequest request)
        => CreateUserAsync(request).GetAwaiter().GetResult();

    public async Task<ServiceResponse<AppUser>> CreateUserAsync(CreateUserRequest request)
    {
        var correctResponse = ValidateCorrectness(request);

        if (!correctResponse.Success)
        {
            correctResponse.AddError(ServiceError.CreationFailed("One or more fields are not valid data"));
            return correctResponse.MutateDroppingData<AppUser>();
        }

        var uniqueResponse = await ValidateUniquenessAsync(request);
        if (!uniqueResponse.Success)
        {
            uniqueResponse.AddError(ServiceError.CreationFailed("One or more key fields are not unique"));
            return uniqueResponse.MutateDroppingData<AppUser>();
        }

        var newUser = request.MapRequest();

        await _context.AddAsync(newUser);
        if (await SaveChangesToDatabaseAsync())
        {
            return ServiceResponse<AppUser>.SuccessResponse("User Created!", newUser);
        }

        return ServiceResponse<AppUser>.ErrorResponse(
            title: "User Creation Failed!",
            error: ServiceError.InternalServerError("Couldn't save user to database.")
        );
    }

    public ServiceResponse<AppUser> UpdateUser(UpdateUserRequest request)
        => UpdateUserAsync(request).GetAwaiter().GetResult();

    public async Task<ServiceResponse<AppUser>> UpdateUserAsync(UpdateUserRequest request)
    {
        var user = await _context.Users.SingleOrDefaultAsync(user => user.Id == request.Id);

        if (user is null)
        {
            return ServiceResponse<AppUser>.ErrorResponse(
                title: "No Such User ID",
                error: ServiceError.UpdateFailed("Cannot update a non-existent user")
            );
        }

        var correctResponse = ValidateCorrectness(request, true);

        if (!correctResponse.Success)
        {
            correctResponse.AddError(ServiceError.UpdateFailed("One or more fields are not valid data"));
            return correctResponse.MutateDroppingData<AppUser>();
        }

        var uniqueResponse = await ValidateUniquenessAsync(request);
        if (!uniqueResponse.Success)
        {
            uniqueResponse.AddError(ServiceError.UpdateFailed("One or more key fields are not unique"));
            return uniqueResponse.MutateDroppingData<AppUser>();
        }

        user.Username = request.Username.IsWhiteSpace() ? user.Username : request.Username;
        user.FullName = request.FullName.IsWhiteSpace() ? user.FullName : request.FullName;
        user.EmailAddress = request.EmailAddress.IsWhiteSpace() ? user.EmailAddress : request.EmailAddress;
        user.SystemUsername = request.SystemUsername.IsWhiteSpace() ? user.SystemUsername : request.SystemUsername;
        user.Gender = request.Gender == UserGender.Unknown ? user.Gender : request.Gender;
        user.BirthDate = request.BirthDate is null ? user.BirthDate : request.BirthDate;

        if (!request.Password.IsWhiteSpace())
        {
            var (hash, salt) = HashUtils.HashPassword(request.Password);
            user.PasswordHash = hash;
            user.PasswordSalt = salt;
        }

        _context.Update(user);

        if (await SaveChangesToDatabaseAsync())
        {
            return new ServiceResponse<AppUser>
            {
                Title = "User Updated Successfully",
                Success = true,
                Data = user,
            };
        }

        return ServiceResponse<AppUser>.ErrorResponse(
            title: "User Update Failed",
            error: ServiceError.InternalServerError("Couldn't update database")
        );
    }

    public ServiceResponse DeleteUser(DeleteUserRequest request)
        => DeleteUserAsync(request).GetAwaiter().GetResult();

    public async Task<ServiceResponse> DeleteUserAsync(DeleteUserRequest request)
    {
        var user = await _context.Users.SingleOrDefaultAsync(user => user.Id == request.Id);

        if (user is null)
        {
            return ServiceResponse.ErrorResponse(
                title: "Deletion Failed",
                error: ServiceError.NotFound("user")
            );
        }

        // var reason = request.Reason;  // TODO: Use this.

        _context.Remove(user);

        if (await SaveChangesToDatabaseAsync())
        {
            return ServiceResponse.SuccessResponse("User Deleted Successfully");
        }

        return ServiceResponse.ErrorResponse(
            title: "User Deletion Failed",
            error: ServiceError.InternalServerError("Couldn't delete the user from the database")
        );
    }

    public ServiceResponse AddToRole(AppUser user, string role)
        => AddToRoleAsync(user, role).GetAwaiter().GetResult();

    public async Task<ServiceResponse> AddToRoleAsync(AppUser user, string role)
    {
        if (!AppRoles.IsKnown(role))
        {
            return ServiceResponse.ErrorResponse(
                "Failed to Add User to Role",
                ServiceError.InvalidData($"Role '{role}' is not found")
            );
        }

        user.Roles.Add(role);
        var success = await SaveChangesToDatabaseAsync();

        if (success)
        {
            return ServiceResponse.SuccessResponse("User Successfully Added to Role");
        }

        return ServiceResponse.ErrorResponse(
            "Failed to Add to Role",
            ServiceError.UpdateFailed("User cannot be added to role")
        );
    }

    public ServiceResponse RemoveFromRole(AppUser user, string role)
        => RemoveFromRoleAsync(user, role).GetAwaiter().GetResult();

    public async Task<ServiceResponse> RemoveFromRoleAsync(AppUser user, string role)
    {
        if (!AppRoles.IsKnown(role))
        {
            return ServiceResponse.ErrorResponse(
                "Failed to Remove User from Role",
                ServiceError.InvalidData($"Role '{role}' is not found")
            );
        }

        var removed = user.Roles.Remove(role);

        if (!removed)
        {
            return ServiceResponse.ErrorResponse(
                "Failed to Remove User from Role",
                ServiceError.NotFound("role")
            );
        }

        var success = await SaveChangesToDatabaseAsync();

        if (success)
        {
            return ServiceResponse.SuccessResponse("User Successfully Removed from Role");
        }

        return ServiceResponse.ErrorResponse(
            "Failed to Add to Role",
            ServiceError.UpdateFailed("User cannot be added to role")
        );
    }
}