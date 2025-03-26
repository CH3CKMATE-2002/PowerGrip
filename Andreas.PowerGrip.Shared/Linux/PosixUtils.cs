namespace Andreas.PowerGrip.Shared.Linux;

// ---------------------------------------------------*
// FIXME: free(): Invalid Pointer                     |
// ---------------------------------------------------*
// FIXED: Was using Marshal.FreeHGlobal(ptr) when the |
// FIXED: libc library was managing the pointer.      |
// ---------------------------------------------------*

public static class PosixUtils
{
    #region Low Level Things

    // chmod returns 0 on success, or -1 on error
    [DllImport("libc", SetLastError = true)]
    private static extern int chmod(string path, uint mode);

    // chown returns 0 on success, or -1 on error
    [DllImport("libc", SetLastError = true)]
    private static extern int chown(string path, uint owner, uint group);

    #endregion

    [Obsolete("Use the built-in methods in the \"File\" class on .NET +7 instead")]
    public static bool ModifyPermissions(string path, UnixFileMode mode)
    {
        Guard.ThrowIfNotLinux($"{nameof(ModifyPermissions)} only works on Linux");

        if (!File.Exists(path))
        {
            return false;
        }

        int res = chmod(path, (uint)mode);
        return res == 0;
    }

    public static bool ChangeOwnership(string path, uint ownerId, uint groupId)
    {
        Guard.ThrowIfNotLinux($"{nameof(ChangeOwnership)} only works on Linux");

        if (!File.Exists(path))
        {
            return false;
        }

        try
        {
            int res = chown(path, ownerId, groupId);
            return res == 0;
        }
        catch
        {
            return false;
        }
    }
}
