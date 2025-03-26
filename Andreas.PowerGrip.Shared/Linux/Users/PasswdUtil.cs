namespace Andreas.PowerGrip.Shared.Linux.Users;

internal class PasswdUtil
{
    #region Low Level Syscalls & Structs
    [DllImport("libc", SetLastError = true)]
    private static extern uint getuid();

    [DllImport("libc", SetLastError = true)]
    private static extern uint geteuid();

    [DllImport("libc", SetLastError = true)]
    private static extern uint getegid();

    [DllImport("libc", SetLastError = true)]
    private static extern IntPtr getgrgid(uint gid);

    [DllImport("libc", SetLastError = true)]
    private static extern IntPtr getpwuid(uint uid);

    [DllImport("libc", SetLastError = true)]
    private static extern IntPtr getgrnam(string name);

    [DllImport("libc", SetLastError = true)]
    private static extern IntPtr getpwnam(string name);

    [DllImport("libc", SetLastError = true)]
    private static extern int getgrouplist(string user, uint base_gid, IntPtr groups, ref int ngroups);

    [StructLayout(LayoutKind.Sequential)]
    internal struct Passwd
    {
        // Add the MarshalAs attribute to avoid Marshal.PtrToStringAnsi calls
        [MarshalAs(UnmanagedType.LPStr)] public string Username;   // pw_name -> user name
        [MarshalAs(UnmanagedType.LPStr)] public string Password;   // pw_passwd -> user password
        public uint UserId;       // pw_uid -> user ID
        public uint GroupId;      // pw_gid -> group ID
        [MarshalAs(UnmanagedType.LPStr)] public string UserInfo;   // pw_gecos -> user info
        [MarshalAs(UnmanagedType.LPStr)] public string HomeDir;    // pw_dir -> home directory
        [MarshalAs(UnmanagedType.LPStr)] public string Shell;      // pw_shell -> shell program
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Group
    {
        [MarshalAs(UnmanagedType.LPStr)] public string Name;
        [MarshalAs(UnmanagedType.LPStr)] public string Password;
        public uint GroupId;
        public IntPtr Members;

        public string[] GetMembers()
        {
            List<string> members = new List<string>();
            IntPtr currentPtr = Members;

            // Loop until we hit a null pointer (terminator)
            while (true)
            {
                IntPtr strPtr = Marshal.ReadIntPtr(currentPtr);
                if (strPtr == IntPtr.Zero)
                    break;

                string? member = Marshal.PtrToStringAnsi(strPtr);
                if (member != null)
                    members.Add(member);

                currentPtr += IntPtr.Size; // Move to next pointer in the array
            }

            return members.ToArray();
        }
    }
    #endregion

    public static Passwd GetPasswdById(uint? uid = null)
    {
        try
        {
            IntPtr pwdPtr = getpwuid(uid ?? geteuid());

            if (pwdPtr == IntPtr.Zero)
            {
                throw new InvalidOperationException("User not found.");
            }

            // return Marshal.PtrToStringAnsi(pwd.UserInfo) ?? string.Empty;
            return Marshal.PtrToStructure<Passwd>(pwdPtr);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to get user by ID.", ex);
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

    public static Passwd GetPasswdByName(string username)
    {
        if (string.IsNullOrEmpty(username))
            throw new ArgumentNullException(nameof(username));

        try
        {
            IntPtr pwdPtr = getpwnam(username);

            if (pwdPtr == IntPtr.Zero)
            {
                throw new InvalidOperationException($"User '{username}' not found.");
            }

            return Marshal.PtrToStructure<Passwd>(pwdPtr);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to get user by name.", ex);
        }
    }

    public static Group GetGroupById(uint? gid = null)
    {
        try
        {
            IntPtr grpPtr = getgrgid(gid ?? getegid()); // What to call here?

            if (grpPtr == IntPtr.Zero)
            {
                throw new InvalidOperationException($"Group with ID {gid} not found.");
            }

            return Marshal.PtrToStructure<Group>(grpPtr);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to get group by ID.", ex);
        }
    }

    public static Group GetGroupByName(string groupName)
    {
        if (string.IsNullOrEmpty(groupName))
            throw new ArgumentNullException(nameof(groupName));

        try
        {
            IntPtr grpPtr = getgrnam(groupName);

            if (grpPtr == IntPtr.Zero)
            {
                throw new InvalidOperationException($"Group '{groupName}' not found.");
            }

            return Marshal.PtrToStructure<Group>(grpPtr);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to get group by name.", ex);
        }
    }

    public static uint[] GetUserGroupIds(string username)
    {
        if (string.IsNullOrEmpty(username))
            throw new ArgumentNullException(nameof(username));

        var passwd = GetPasswdByName(username);
        uint primaryGid = passwd.GroupId;

        int ngroups = 0;
        // First call to determine required buffer size
        int result = getgrouplist(username, primaryGid, IntPtr.Zero, ref ngroups);

        if (result != -1 && ngroups <= 0)
            return Array.Empty<uint>();

        IntPtr groupsBuffer = IntPtr.Zero;
        try
        {
            groupsBuffer = Marshal.AllocHGlobal(ngroups * sizeof(int));
            int bufferSize = ngroups;

            result = getgrouplist(username, primaryGid, groupsBuffer, ref bufferSize);

            if (result == -1)
                throw new InvalidOperationException("Failed to retrieve user groups.");

            uint[] groups = new uint[bufferSize];
            for (int i = 0; i < bufferSize; i++)
            {
                groups[i] = (uint)Marshal.ReadInt32(groupsBuffer, i * sizeof(int));
            }

            return groups;
        }
        finally
        {
            if (groupsBuffer != IntPtr.Zero)
                Marshal.FreeHGlobal(groupsBuffer);
        }
    }

    public static uint[] GetUserGroupIds(uint? uid = null)
    {
        var passwd = GetPasswdById(uid ?? geteuid());
        uint primaryGid = passwd.GroupId;
        string username = passwd.Username;

        int ngroups = 0;
        // First call to determine required buffer size
        int result = getgrouplist(username, primaryGid, IntPtr.Zero, ref ngroups);

        if (result != -1 && ngroups <= 0)
            return Array.Empty<uint>();

        IntPtr groupsBuffer = IntPtr.Zero;
        try
        {
            groupsBuffer = Marshal.AllocHGlobal(ngroups * sizeof(int));
            int bufferSize = ngroups;

            result = getgrouplist(username, primaryGid, groupsBuffer, ref bufferSize);

            if (result == -1)
                throw new InvalidOperationException("Failed to retrieve user groups.");

            uint[] groups = new uint[bufferSize];
            for (int i = 0; i < bufferSize; i++)
            {
                groups[i] = (uint)Marshal.ReadInt32(groupsBuffer, i * sizeof(int));
            }

            return groups;
        }
        finally
        {
            if (groupsBuffer != IntPtr.Zero)
                Marshal.FreeHGlobal(groupsBuffer);
        }
    }

    public static uint GetUserID()
    {
        try
        {
            return getuid();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"System call to {nameof(getuid)} failed", ex);
        }
    }

    public static uint GetEffectiveUserId()
    {
        try
        {
            return geteuid();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"System call to {nameof(geteuid)} failed", ex);
        }
    }

    public static uint GetPrimaryGroupId()
    {
        try
        {
            return getegid();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"System call to {nameof(getuid)} failed", ex);
        }
    }
}