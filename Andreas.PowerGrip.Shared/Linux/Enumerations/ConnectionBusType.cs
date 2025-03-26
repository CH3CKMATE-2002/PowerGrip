namespace Andreas.PowerGrip.Shared.Linux.Enumerations;

public enum ConnectionBusType
{
    [EnumMember(Value = "usb")] Usb,
    [EnumMember(Value = "sdio")] SecureDigitalIO,
    [EnumMember(Value = "ieee1394")] FireWire,
    [EnumMember(Value = "unknown")] Unknown
}