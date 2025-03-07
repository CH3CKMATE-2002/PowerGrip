namespace Andreas.PowerGrip.Shared.Utilities;

// ---------------------------------------------------*
// FIXME: free(): Invalid Pointer                     |
// ---------------------------------------------------*
// FIXED: Was using Marshal.FreeHGlobal(ptr) when the |
// FIXED: libc library was managing the pointer.      |
// ---------------------------------------------------*

public static class UnixUtils
{
    #region Low Level Things
    [DllImport("libc", SetLastError = true)]
    private static extern int getuid();

    [DllImport("libc", SetLastError = true)]
    private static extern int geteuid();

    [DllImport("libc", SetLastError = true)]
    private static extern IntPtr getpwuid(int uid);

    // chmod returns 0 on success, or -1 on error
    [DllImport("libc", SetLastError = true)]
    private static extern int chmod(string path, uint mode);

    // chown returns 0 on success, or -1 on error
    [DllImport("libc", SetLastError = true)]
    private static extern int chown(string path, int owner, int group);

    // Import getgrnam from libc. It returns a pointer to a native 'group' struct.
    [DllImport("libc", SetLastError = true)]
    private static extern IntPtr getgrnam(string name);

    // Import getpwnam from libc. It returns a pointer to a native 'passwd' struct.
    [DllImport("libc", SetLastError = true)]
    private static extern IntPtr getpwnam(string name);

    [DllImport("libc")]
    private static extern int uname(out Utsname buf);

    [DllImport("libc")]
    private static extern int sysinfo(out Sysinfo info);

    // Represents the native passwd struct (from <pwd.h>).
    [StructLayout(LayoutKind.Sequential)]
    private struct Passwd
    {
        public IntPtr Username;   // pw_name -> user name
        public IntPtr Password;   // pw_passwd -> user password
        public uint UserId;       // pw_uid -> user ID
        public uint GroupId;      // pw_gid -> group ID
        public IntPtr UserInfo;   // pw_gecos -> user info
        public IntPtr HomeDir;    // pw_dir -> home directory
        public IntPtr Shell;      // pw_shell -> shell program
    }

    // Represents the native group struct (from <grp.h>).
    [StructLayout(LayoutKind.Sequential)]
    private struct Group
    {
        public IntPtr Name;       // gr_name -> group name
        public IntPtr Password;   // gr_passwd -> group password
        public uint GroupId;      // gr_gid -> group ID
        public IntPtr Members;    // gr_mem -> pointer to member list (unused here)
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct Utsname
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)]
        public string SysName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)]
        public string NodeName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)]
        public string Release;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)]
        public string Version;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)]
        public string Machine;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)]
        public string DomainName;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct Sysinfo
    {
        public long Uptime;
        public ulong Load1, Load5, Load15;
        public ulong TotalRam, FreeRam, SharedRam, BufferRam;
        public ulong TotalSwap, FreeSwap;
        public ushort Procs;
        public ulong TotalHigh, FreeHigh;
        public uint MemUnit;
    }
    #endregion
    
    /// <summary>
    /// Checks if the user is root or not.
    /// </summary>
    /// <returns>True if the user is root, otherwise False</returns>
    /// <exception cref="InvalidOperationException"></exception> <summary>
    /// Occurs if the call to the native <code>getuid</code> fails
    /// </summary>
    public static bool IsRoot()
    {
        try
        {
            return getuid() == 0;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"System call to {nameof(getuid)} failed", ex);
        }
    }

    /// <summary>
    /// Checks if the effective permissions are for root.
    /// </summary>
    /// <returns>True if the program has root permissions, otherwise false</returns>
    /// <exception cref="InvalidOperationException"></exception> <summary>
    /// Occurs if the call to the native <code>geteuid</code> fails
    /// </summary>
    public static bool HasRootPermissions()
    {
        try
        {
            return geteuid() == 0;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"System call to {nameof(geteuid)} failed", ex);
        }
    }

    public static string GetUsername()
    {
        IntPtr pwdPtr = IntPtr.Zero;

        try
        {
            int uid = geteuid();
            pwdPtr = getpwuid(uid);

            if (pwdPtr == IntPtr.Zero)
            {
                throw new InvalidOperationException("The operation failed.");
            }

            Passwd pwd = Marshal.PtrToStructure<Passwd>(pwdPtr);
            return Marshal.PtrToStringAnsi(pwd.Username) ?? string.Empty;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("The operation failed.", ex);
        }
        //! WARNING: This is un-needed here as the pointers here are managed by libc.
        // finally
        // {
        //     if (pwdPtr != IntPtr.Zero)
        //     {
        //         Marshal.FreeHGlobal(pwdPtr);
        //     }
        // }
    }

    /// <summary>
    /// Returns the numeric group ID (GID) for the specified group name.
    /// Throws an exception if the group cannot be found.
    /// </summary>
    public static int GetGroupId(string groupName)
    {
        IntPtr grpPtr = IntPtr.Zero;

        try
        {
            grpPtr = getgrnam(groupName);

            if (grpPtr == IntPtr.Zero)
                throw new Exception($"Group '{groupName}' not found.");

            Group grp = Marshal.PtrToStructure<Group>(grpPtr);
            return (int)grp.GroupId;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("The operation failed.", ex);
        }
        //! WARNING: This is un-needed here as the pointers here are managed by libc.
        // finally
        // {
        //     if (grpPtr != IntPtr.Zero)
        //     {
        //         Marshal.FreeHGlobal(grpPtr);
        //     }
        // }
    }

    /// <summary>
    /// Returns the numeric user ID (UID) for the specified user name.
    /// Throws an exception if the user cannot be found.
    /// </summary>
    public static int GetUserId(string userName)
    {
        IntPtr pwdPtr = IntPtr.Zero;

        try
        {
            pwdPtr = getpwnam(userName);
            if (pwdPtr == IntPtr.Zero)
                throw new Exception($"User '{userName}' not found.");

            Passwd pwd = Marshal.PtrToStructure<Passwd>(pwdPtr);
            return (int)pwd.UserId;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("The operation failed.", ex);
        }
        //! WARNING: This is un-needed here as the pointers here are managed by libc.
        // finally
        // {
        //     if (pwdPtr != IntPtr.Zero)
        //     {
        //         Marshal.FreeHGlobal(pwdPtr);
        //     }
        // }
    }

    public static bool ModifyPermissions(string path, UnixFileMode mode)
    {
        if (!File.Exists(path))
        {
            return false;
        }

        int res = chmod(path, (uint)mode);
        return res == 0;
    }

    public static bool ChangeOwnership(string path, string owner, string group)
    {
        try
        {
            int ownerId = GetUserId(owner);
            int groupId = GetGroupId(group);

            int res = chown(path, ownerId, groupId);
            return res == 0;
        }
        catch
        {
            return false;
        }
    }

    public static string GetKernelRelease()
        => uname(out Utsname uts) == 0 ? uts.Release : "Unknown";
    
    public static string GetKernelName()
        => uname(out Utsname uts) == 0 ? uts.SysName : "Unknown";
    
    public static string GetHostname()
        => uname(out Utsname uts) == 0 ? uts.NodeName : "Unknown";
    
    public static string GetKernelVersion()
        => uname(out Utsname uts) == 0 ? uts.Version : "Unknown";
    
    public static string GetMachineTypeName()
        => uname(out Utsname uts) == 0 ? uts.Machine : "unknown";
    
    public static string GetDomainName()
        => uname(out Utsname uts) == 0 ? uts.DomainName : "Unknown";
    
    public static KernelInfo GetKernelInfo()
    {
        if (uname(out Utsname uts) == 0)
        {
            return new KernelInfo
            {
                Name = uts.SysName,
                Hostname = uts.NodeName,
                Version = uts.Version,
                Release = uts.Release,
                MachineType = MachineTypeUtils.FromString(uts.Machine),
                DomainName = uts.DomainName
            };
        }

        return new KernelInfo();
    }
    
    public static TimeSpan GetUptime()
        => sysinfo(out Sysinfo info) == 0 ? TimeSpan.FromSeconds(info.Uptime) : TimeSpan.Zero;
}
