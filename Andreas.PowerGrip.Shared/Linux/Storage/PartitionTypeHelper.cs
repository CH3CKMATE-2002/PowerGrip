namespace Andreas.PowerGrip.Shared.Linux.Storage;

public static class PartitionTypeHelper
{
    private static readonly Dictionary<Guid, string> GptTypeMap = new()
    {
        { Guid.Parse("C12A7328-F81F-11D2-BA4B-00A0C93EC93B"), "EFI System Partition" },
        { Guid.Parse("0FC63DAF-8483-4772-8E79-3D69D8477DE4"), "Linux Filesystem" }
    };

    private static readonly Dictionary<byte, string> MbrTypeMap = new()
    {
        { 0x83, "Linux Filesystem" },
        { 0xEF, "EFI System Partition" },
        { 0x07, "NTFS/HPFS" }
    };

    public static string GetHumanReadableType(PartitionTableKind tableKind, string typeCode)
    {
        return tableKind switch
        {
            PartitionTableKind.GuidPartitionTable
                => GptTypeMap.GetValueOrDefault(Guid.Parse(typeCode), "Unknown GPT Type"),
            PartitionTableKind.MasterBootRecord
                => MbrTypeMap.GetValueOrDefault(byte.Parse(typeCode.Replace("0x", ""), NumberStyles.HexNumber), "Unknown MBR Type"),
            _ => "Unknown"
        };
    }
}