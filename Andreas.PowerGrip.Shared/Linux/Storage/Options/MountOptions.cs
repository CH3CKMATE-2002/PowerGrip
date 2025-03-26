namespace Andreas.PowerGrip.Shared.Linux.Storage.Options;

public class MountOptions : UDisks2OptionsBase
{
    /// <summary>
    /// The desired mount point for the filesystem.
    /// </summary>
    /// <remarks>
    /// PLEASE note that UDisks2 does not currently support
    /// specifying a custom mount path via this property. The filesystem will be mounted at an
    /// automatically determined path (usually under <c>/run/media/$USER/</c>). This property
    /// is reserved for future compatibility if UDisks2 adds support for custom mount paths.
    /// </remarks>
    public string MountPoint { get; set; } = string.Empty;

    public MountingFlags Flags { get; set; } = MountingFlags.None;

    public FileSystemKind FsType { get; set; } = FileSystemKind.Unknown;

    public string AsUsername { get; set; } = string.Empty;

    public override Dictionary<string, object> ToDictionary()
    {
        var result = base.ToDictionary();

        if (FsType is not FileSystemKind.Unknown)
        {
            result.Add("fstype", FsType.ToString());
        }

        var flags = FlagString();

        if (!string.IsNullOrWhiteSpace(flags))
        {
            result.Add("options", flags);
        }

        if (!string.IsNullOrWhiteSpace(AsUsername))
        {
            result.Add("as-user", AsUsername);
        }

        if (!string.IsNullOrWhiteSpace(MountPoint))
        {
            result.Add("mount_point", MountPoint);
        }

        return result;
    }

    private string FlagString()
    {
        if (Flags == MountingFlags.None)
        {
            return "";
        }

        List<string> flags = [];

        if (Flags.HasFlag(MountingFlags.ReadWrite)) flags.Add("rw");
        if (Flags.HasFlag(MountingFlags.Relatime)) flags.Add("relatime");
        if (Flags.HasFlag(MountingFlags.NoExec)) flags.Add("noexec");
        if (Flags.HasFlag(MountingFlags.NoSuid)) flags.Add("nosuid");

        return string.Join(',', flags);
    }
}