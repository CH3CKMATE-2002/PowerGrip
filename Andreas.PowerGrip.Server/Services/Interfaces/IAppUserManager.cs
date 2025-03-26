namespace Andreas.PowerGrip.Server.Services.Interfaces;

/// <summary>
/// An interface that defines the expected functionality of
/// a user manager store.
/// </summary>
public interface IAppUserManager
{
    /// <summary>
    /// A method to retrieve an <see cref="AppUser" /> from the
    /// underlying <see cref="AppDbContext" />.
    /// </summary>
    /// <param name="id">The ID of the wanted <see cref="AppUser" /></param>
    /// <returns>
    /// The <see cref="AppUser" /> with the given Id, or null if it doesn't exist.
    /// </returns>
    AppUser? GetUserById(Guid id);

    Task<AppUser?> GetUserByIdAsync(Guid id);

    AppUser? GetUserByUsername(string username);

    Task<AppUser?> GetUserByUsernameAsync(string username);

    AppUser? GetUserByEmail(string email);

    Task<AppUser?> GetUserByEmailAsync(string email);

    bool CheckPassword(AppUser user, string password);

    AuthResponse Login(AppUser user);

    Task<AuthResponse> LoginAsync(AppUser user);

    ServiceResponse Logout(AppUser user, PgLogoutRequest request);

    Task<ServiceResponse> LogoutAsync(AppUser user, PgLogoutRequest request);

    AuthResponse RefreshSession(RefreshRequest request);

    Task<AuthResponse> RefreshSessionAsync(RefreshRequest request);

    bool IsInRole(AppUser user, string role);

    Task<bool> IsInRoleAsync(AppUser user, string role);

    List<string> GetRoles(AppUser user);

    Task<List<string>> GetRolesAsync(AppUser user);
    
    ServiceResponse<AppUser> CreateUser(CreateUserRequest request);

    Task<ServiceResponse<AppUser>> CreateUserAsync(CreateUserRequest request);

    ServiceResponse<AppUser> UpdateUser(UpdateUserRequest request);

    Task<ServiceResponse<AppUser>> UpdateUserAsync(UpdateUserRequest request);

    ServiceResponse DeleteUser(DeleteUserRequest request);

    Task<ServiceResponse> DeleteUserAsync(DeleteUserRequest request);

    ServiceResponse AddToRole(AppUser user, string role);

    Task<ServiceResponse> AddToRoleAsync(AppUser user, string role);

    ServiceResponse RemoveFromRole(AppUser user, string role);

    Task<ServiceResponse> RemoveFromRoleAsync(AppUser user, string role);
}