namespace Andreas.PowerGrip.Shared.Linux;

public class UnixProcess
{
    #region Low Level Sys-calls

    [DllImport("libc", SetLastError = true)]
    private static extern int getpid();  // This process

    [DllImport("libc", SetLastError = true)]
    private static extern int getppid();  // Parent process

    [DllImport("libc", SetLastError = true)]
    private static extern int kill(int pid, int sig);

    [DllImport("libc", SetLastError = true)]
    private static extern int setpriority(int which, int who, int prio);

    [DllImport("libc", SetLastError = true)]
    private static extern int getpriority(int which, int who);

    [DllImport("libc", SetLastError = true)]
    private static extern int posix_spawn(
        out int pid, string path,
        IntPtr fileActions, IntPtr attrp,
        string[] argv, string[]? envp);

    [DllImport("libc", SetLastError = true)]
    private static extern int waitpid(int pid, out int status, int options);

    /// <summary>
    /// Error code that indicates the process is gone.
    /// </summary>
    private const int ESRCH = 3;

    /// <summary>
    /// Error code that indicates that the process is not this process' child.
    /// </summary>
    private const int ECHILD = 10;

    /// <summary>
    /// Non-blocking mode for <c>waitpid</c>.
    /// </summary>
    private const int WNOHANG = 1; // Non-blocking mode for waitpid

    #endregion

    #region Attributes
    /// <summary>
    /// The process ID on the system.
    /// </summary>
    /// <value></value>
    public int Id { get; }

    /// <summary>
    /// The process' parent ID on the system.
    /// </summary>
    /// <value></value>
    public int ParentId { get; private set; }

    /// <summary>
    /// The process current state (e.g. Running, Stopped, Zombie, etc...).
    /// </summary>
    /// <value></value>
    public UnixProcessState State { get; private set; }

    /// <summary>
    /// The process name as it appears on the system. (Limited to 16 characters)
    /// </summary>
    /// <value></value>
    public string? Name { get; private set; }

    public int Umask { get; private set; }

    public int ThreadGroupId { get; private set; }

    public int TracerProcessId { get; private set; }

    private string? _cachedCommandLine;

    /// <summary>
    /// The commandline used to invoke the process.
    /// </summary>
    /// <value></value>
    public string CommandLine => _cachedCommandLine ??= ReadCommandLine();

    /// <summary>
    /// The process executable's path.
    /// </summary>
    /// <value></value>
    public string? ExecutablePath => Directory.ResolveLinkTarget($"/proc/{Id}/exe", true)?.FullName;

    /// <summary>
    /// The name of the process executable.
    /// </summary>
    /// <value></value>
    public string? ExecutableName => Path.GetFileName(ExecutablePath);

    /// <summary>
    /// The current working directory of the process.
    /// </summary>
    /// <value></value>
    public string? CurrentWorkingDirectory => Directory.ResolveLinkTarget($"/proc/{Id}/cwd", true)?.FullName;

    /// <summary>
    /// The real user ID of the process owner
    /// </summary>
    /// <value></value>
    public int UserId { get; private set; }

    public int EffectiveUserId { get; private set; }

    public int SavedUserId { get; private set; }

    public int FilesystemUserId { get; private set; }

    /// <summary>
    /// The real group ID of the process owner
    /// </summary>
    /// <value></value>
    public int GroupId { get; private set; }

    public int EffectiveGroupId { get; private set; }

    public int SavedGroupId { get; private set; }

    public int FilesystemGroupId { get; private set; }

    public int FileDescriptorCount { get; private set; }

    public int[] GroupIds { get; private set; } = [];

    public ulong VirtualMemorySize { get; private set; }

    public int ThreadCount { get; private set; }

    public ulong BytesRead { get; private set; }

    public ulong BytesWritten { get; private set; }

    /// <summary>
    /// Gets or sets the priority of the process (nice value: -20 to 19)
    /// </summary>
    /// <value></value>
    public int Priority
    {
        get
        {
            int result = getpriority(0, Id);
            int errorNo = Marshal.GetLastWin32Error();
            return errorNo == 0 ? result : throw new Exception($"Failed to get priority (code: {errorNo})");
        }
        set => setpriority(0, Id, Math.Clamp(value, -20, 19));
    }

    /// <summary>
    /// The path to the info file the process was read from.
    /// </summary>
    /// <returns></returns>
    public string InfoPath => Path.Join("/proc", Id.ToString());

    /// <summary>
    /// Returns whether the process exists on the system currently or not.
    /// </summary>
    /// <returns></returns>
    public bool Exists => Directory.Exists(InfoPath);

    public ulong ResidentSetSize { get; private set; } // Actual RAM usage

    public ulong SwapSize { get; private set; } // Swap used by the process
    
    #endregion

    /// <summary>
    /// Reads and constructs an <see cref="UnixProcess"/> object by using a given PID.
    /// </summary>
    /// <param name="id">The process ID of the existing process</param>
    private UnixProcess(int id)
    {
        Id = id;

        if (!this.Exists)
        {
            throw new ArgumentException($"No such process with PID at the moment: {id}");
        }

        Update();
    }

    /// <summary>
    /// Kills the underlying process. (Ungraceful Termination)
    /// </summary>
    /// <returns><c>true</c> if the process was killed, otherwise <c>false</c></returns>
    public bool Kill() => SendSignal(UnixSignal.Kill);

    /// <summary>
    /// Gracefully terminates the underlying process.
    /// </summary>
    /// <returns><c>true</c> if the process was terminated, otherwise <c>false</c></returns>
    public bool Terminate() => SendSignal(UnixSignal.Terminate);

    public bool Stop() => SendSignal(UnixSignal.Stop);

    public bool Continue() => SendSignal(UnixSignal.Continue);

    /// <summary>
    /// Sends a signal to the process for various tasks.
    /// </summary>
    /// <param name="signal">The <see cref="UnixSignal"/> to be sent to the process.</param>
    /// <returns><c>true</c> if the signal was sent and succeeded, otherwise <c>false</c></returns>
    public bool SendSignal(UnixSignal signal) => kill(Id, (int)signal) == 0;

    public void WaitForExit()
    {
        // First, try "waitpid":
        int status;
        int result = waitpid(Id, out status, 0);

        if (result == Id)
        {
            return;  // Zombie process reaped!
        }
        else
        {
            int err = Marshal.GetLastWin32Error();
            if (err == ECHILD)
            {
                // Fallback: Poll /proc status
                PollUntilExit();
            }
            else
            {
                throw new Exception($"waitpid failed (code: {err})");
            }
        }
    }

    private void PollUntilExit()
    {
        const int ESRCH = 3;
        int sleepTime = 100;

        while (true)
        {
            int killResult = kill(Id, 0);
            if (killResult == -1 && Marshal.GetLastWin32Error() == ESRCH)
                break;

            Thread.Sleep(sleepTime);
            sleepTime = Math.Min(sleepTime + 100, 1000);
        }
    }

    public Task WaitForExitAsync() => Task.Run(WaitForExit);

    public static UnixProcess? Start(string filename, params string[] args)
    {
        string[] argv = new string[args.Length + 2];
        
        argv[0] = filename; // argv[0] must be the executable name

        Array.Copy(args, 0, argv, 1, args.Length); // Filling the argv

        argv[args.Length + 1] = null!; // NULL termination

        int result = posix_spawn(out int pid, filename, IntPtr.Zero, IntPtr.Zero, argv, null);

        if (result != 0)
        {
            throw new Exception($"posix_spawn failed with error code {result}");
        }

        return GetProcess(pid);
    }

    public static UnixProcess? GetProcess(int? id = null)
    {
        try
        {
            return new UnixProcess(id ?? getpid());
        }
        catch
        {
            return null;
        }
    }

    public static UnixProcess GetParentProcess() => new(getppid());

    public static UnixProcess[] GetAllProcesses()
    {
        return Directory.GetDirectories("/proc")
                        .Select(dir => Path.GetFileName(dir))
                        .Where(name => name!.All(char.IsDigit))
                        .Select(pidStr => int.Parse(pidStr))
                        .Where(pid => Directory.Exists($"/proc/{pid}")) // Prevent race condition
                        .Select(pid => UnixProcess.GetProcess(pid)!)
                        .Where(proc => proc is not null)
                        .ToArray();
    }

    public override string ToString()
    {
        return $"{ExecutableName} ({Id})";
    }

    private Dictionary<string, string> ReadProcessStatus()
    {
        Dictionary<string, string> result = [];

        string statusPath = $"/proc/{Id}/status";
        string ioPath = $"/proc/{Id}/io";

        List<string> lines = [];

        if (File.Exists(statusPath)) lines.AddRange(File.ReadAllLines(statusPath));
        if (File.Exists(ioPath)) lines.AddRange(File.ReadAllLines(ioPath));

        return lines
                   .Select(line => line.Split(':', 2, StringSplitOptions.TrimEntries))
                   .Where(parts => parts.Length == 2)
                   .ToDictionary(parts => parts[0], parts => parts[1]);
    }

    private static UnixProcessState GetState(string state)
    {
        char c = state[0];
        return c switch
        {
            'R' => UnixProcessState.Running,
            'S' => UnixProcessState.Sleeping,
            'D' => UnixProcessState.UninterruptibleSleep,
            'T' => UnixProcessState.Stopped,
            't' => UnixProcessState.TracingStop,
            'Z' => UnixProcessState.Zombie,
            'X' => UnixProcessState.Dead,
            'K' => UnixProcessState.WakeKill,
            'W' => UnixProcessState.Waking,
            'I' => UnixProcessState.Idle,
            _ => UnixProcessState.Unknown,
        };
    }

    private string ReadCommandLine()
    {
        string path = $"/proc/{Id}/cmdline";
        return File.Exists(path) ? File.ReadAllText(path).Replace('\0', ' ').Trim() : string.Empty;
    }

    public void Update()
    {
        var dict = ReadProcessStatus();

        Name = dict["Name"];
        ParentId = int.Parse(dict["PPid"]);

        Umask = Convert.ToInt32(dict["Umask"], 8);

        State = GetState(dict["State"]);

        ThreadGroupId = int.Parse(dict["Tgid"]);

        TracerProcessId = int.Parse(dict["TracerPid"]);

        var uidFields = dict["Uid"].Split(['\t', ' '], 4, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var gidFields = dict["Gid"].Split(['\t', ' '], 4, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        UserId = int.Parse(uidFields[0]);
        EffectiveUserId = int.Parse(uidFields[1]);
        SavedUserId = int.Parse(uidFields[2]);
        FilesystemUserId = int.Parse(uidFields[3]);

        GroupId = int.Parse(gidFields[0]);
        EffectiveGroupId = int.Parse(gidFields[1]);
        SavedGroupId = int.Parse(gidFields[2]);
        FilesystemGroupId = int.Parse(gidFields[3]);

        FileDescriptorCount = int.Parse(dict["FDSize"]);

        GroupIds = dict["Groups"].Split(' ').Select(int.Parse).ToArray();

        VirtualMemorySize = ulong.Parse(dict["VmSize"].Split()[0]) * 1024;

        ThreadCount = int.Parse(dict["Threads"]);

        BytesRead = ulong.Parse(dict["rchar"]);
        BytesWritten = ulong.Parse(dict["wchar"]);

        ResidentSetSize = ulong.Parse(dict["VmRSS"].Split()[0]) * 1024; // Convert from KB to bytes
        SwapSize = ulong.Parse(dict["VmSwap"].Split()[0]) * 1024;
    }
}