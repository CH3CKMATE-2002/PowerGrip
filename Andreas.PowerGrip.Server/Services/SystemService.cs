namespace Andreas.PowerGrip.Server.Services;

[Obsolete("Do not use anymore. Kept for the record")]
public class SystemService: ISystemService
{
    private readonly ILogger<ISystemService> _logger;

    public const string UsersFilePath = "/etc/passwd";

    public const string HelperPath = "Assets/pam_helper";

    public SystemService(ILogger<ISystemService> logger)
    {
        _logger = logger;
        if (!File.Exists(HelperPath))
        {
            _logger.LogCritical(
                "Helper program does not exists at '{path}'. Please (Re)compile the helper",
                HelperPath
            );
        }
    }

    private LaunchedProcessData RunHelper(string username, string password, string command = "", params string[] args)
    {
        _logger.LogDebug("{method} was called", nameof(RunHelper));

        Stopwatch stp = new();
        StringBuilder arguments = new(username);

        if (!string.IsNullOrEmpty(command))
        {
            arguments.Append(" ").Append(command);
        }

        if (args is not null && args.Length > 0)
        {
            arguments.Append(string.Join(" ", args));
        }

        var argsString = arguments.ToString();

        _logger.LogDebug("Running helper with arguments: {args}", argsString);

        var startInfo = new ProcessStartInfo
        {
            FileName = HelperPath,
            Arguments = argsString,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process { StartInfo = startInfo };
        stp.Start();
        process.Start();

        _logger.LogDebug("Helper process started!");
        using (var writer = process.StandardInput)
        {
            writer.WriteLine(password); // Send password to stdin
        }

        process.WaitForExit();
        stp.Stop();

        _logger.LogDebug("Helper process exited with code: {code}", process.ExitCode);
        return new LaunchedProcessData
        {
            ExitCode = process.ExitCode,
            StdError = process.StandardError.ReadToEnd(),
            StdOutput = process.StandardOutput.ReadToEnd(),
            RunningTime = stp.Elapsed,
        };
    }

    public bool IsSystemUser(string username)
    {
        _logger.LogDebug("Checking if the username is valid: {username}", username);
        try
        {
            _logger.LogDebug("Reading file '{file}'", UsersFilePath);
            return File.ReadAllLines(UsersFilePath).Any(line => line.StartsWith($"{username}:"));
        }
        catch
        {
            _logger.LogDebug("No such username on this server: {username}", username);
            return false;
        }
    }

    public bool Authenticate(PgLoginRequest request)
    {
        //? Can we login on 'sudo' using another way?
        Guard.Assert(request.LoginMethod != LoginMethod.PasswordLogin,
            "Must be password login method only!");

        if (string.IsNullOrEmpty(request.Username) || request.Password is null)
        {
            return false;
        }

        var data = RunHelper(request.Username, request.Password);

        return data.ExitCode == 0;
    }

    public LaunchedProcessData UpdateSystem(PgLoginRequest request)
    {
        //? Can we login on 'sudo' using another way?
        Guard.Assert(request.LoginMethod != LoginMethod.PasswordLogin,
            "Must be password login method only!");
        
        if (string.IsNullOrEmpty(request.Username) || request.Password is null)
        {
            return LaunchedProcessData.FailedToLaunch(HelperPath);
        }

        var data = RunHelper(
            username: request.Username,
            password: request.Password,
            command: "apt",
            args:
            [
                "update"
            ]
        );

        return data;
    }

    public LaunchedProcessData UpgradeSystem(PgLoginRequest request)
    {
        //? Can we login on 'sudo' using another way?
        Guard.Assert(request.LoginMethod != LoginMethod.PasswordLogin,
            "Must be password login method only!");
        
        if (string.IsNullOrEmpty(request.Username) || request.Password is null)
        {
            return LaunchedProcessData.FailedToLaunch(HelperPath);
        }

        var data = RunHelper(
            username: request.Username,
            password: request.Password,
            command: "apt",
            args:
            [
                "upgrade",
                "-y"
            ]
        );

        return data;
    }
}