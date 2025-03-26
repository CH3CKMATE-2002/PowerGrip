namespace Andreas.PowerGrip.Shared.Linux.Users;

public class UnixGroup
{
    public uint GroupId { get; set; }
    public string Name { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;
    public string[] MemberNames { get; private set; } = [];
    
    private UnixGroup() { }

    public UnixUser[] GetAllMembers()
        => MemberNames.Select(UnixUser.GetUser)
            .ToArray();

    public override string ToString()
    {
        return Name;
    }

    public static UnixGroup GetGroup(uint gid)
    {
        var group = PasswdUtil.GetGroupById(gid);
        return BuildFromGroupStruct(group);
    }

    public static UnixGroup GetGroup(string groupName)
    {
        var group = PasswdUtil.GetGroupByName(groupName);
        return BuildFromGroupStruct(group);
    }

    private static UnixGroup BuildFromGroupStruct(PasswdUtil.Group group)
    {
        var result = new UnixGroup
        {
            GroupId = group.GroupId,
            Name = group.Name,
            Password = group.Password,
            MemberNames = group.GetMembers()
        };
        return result;
    }
}