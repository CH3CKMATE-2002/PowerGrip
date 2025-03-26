namespace Andreas.PowerGrip.Shared.Linux.Users;

public class UnixUser
{
    public uint UserId { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public string Realname { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public UnixGroup PrimaryGroup { get; private set; } = null!;
    public string HomeDirectory { get; private set; } = string.Empty;
    public string ShellPath { get; private set; } = string.Empty;

    public bool IsRoot => UserId == 0;

    private UnixUser() { }

    public UnixGroup[] GetAllGroups()
        => PasswdUtil.GetUserGroupIds(Username)
            .Select(UnixGroup.GetGroup)
            .ToArray();

    public bool TakeOwnership(string path)
        => PosixUtils.ChangeOwnership(path, UserId, PrimaryGroup.GroupId);

    public override string ToString()
    {
        return Username;
    }

    public static UnixUser GetUser(uint uid)
    {
        var passwd = PasswdUtil.GetPasswdById(uid);
        return BuildFromPasswdStruct(passwd);
    }

    public static UnixUser GetUser(string username)
    {
        var passwd = PasswdUtil.GetPasswdByName(username);
        return BuildFromPasswdStruct(passwd);
    }

    public static UnixUser GetCurrentUser()
        => GetUser(GetEffectiveUserId());

    public static uint GetCurrentUserId()
        => PasswdUtil.GetUserID();

    public static uint GetEffectiveUserId()
        => PasswdUtil.GetEffectiveUserId();

    private static UnixUser BuildFromPasswdStruct(PasswdUtil.Passwd passwd)
    {
        var result = new UnixUser
        {
            UserId = passwd.UserId,
            Username = passwd.Username,
            Realname = passwd.UserInfo.TrimEnd(','),
            // This should not be valid as passwords are stored in /etc/shadow:
            Password = passwd.Password,
            PrimaryGroup = UnixGroup.GetGroup(passwd.GroupId),
            Description = passwd.UserInfo,
            HomeDirectory = passwd.HomeDir,
            ShellPath = passwd.Shell,
        };

        return result;
    }
}