namespace Andreas.PowerGrip.Shared.Linux.Enumerations;

/// <summary>
/// Represents the architecture or type of a machine.
/// </summary>
public enum MachineArchitecture : ushort
{
    Custom = 0xFFFF,
    Unknown = 0,

    X86,
    X86_64,
    IA64,
    Arm,
    ArmV7,
    Arm64,
    PowerPc,
    PowerPc64,
    PowerPc64LE,
    S390,
    S390X,
    Mips,
    Mips64,
    Sparc,
    Sparc64,
    RiscV,
    RiscV64,

    Alpha,
    PaRisc,
    Motorola68K,
    SuperH,
    Tile,
    TilePro,
    TileGx,
    Cris,
    Arc,
    MicroBlaze,
    Nios2,
    Xtensa
}

public static class ArchitectureUtils
{
    public static MachineArchitecture Parse(string arch) => arch switch
    {
        "i386" or "i486" or "i586" or "i686" => MachineArchitecture.X86,
        "x86_64" or "amd64" => MachineArchitecture.X86_64,
        "ia64" => MachineArchitecture.IA64,
        "arm" => MachineArchitecture.Arm,
        "armv7l" => MachineArchitecture.ArmV7,
        "aarch64" or "arm64" => MachineArchitecture.Arm64,
        "ppc" or "powerpc" => MachineArchitecture.PowerPc,
        "ppc64" => MachineArchitecture.PowerPc64,
        "ppc64le" => MachineArchitecture.PowerPc64LE,
        "s390" => MachineArchitecture.S390,
        "s390x" => MachineArchitecture.S390X,
        "mips" => MachineArchitecture.Mips,
        "mips64" => MachineArchitecture.Mips64,
        "sparc" => MachineArchitecture.Sparc,
        "sparc64" => MachineArchitecture.Sparc64,
        "riscv" => MachineArchitecture.RiscV,
        "riscv64" => MachineArchitecture.RiscV64,
        "alpha" => MachineArchitecture.Alpha,
        "parisc" or "hppa" => MachineArchitecture.PaRisc,
        "m68k" => MachineArchitecture.Motorola68K,
        "sh" => MachineArchitecture.SuperH,
        "tile" => MachineArchitecture.Tile,
        "cris" => MachineArchitecture.Cris,
        "arc" => MachineArchitecture.Arc,
        "microblaze" => MachineArchitecture.MicroBlaze,
        "nios2" => MachineArchitecture.Nios2,
        "xtensa" => MachineArchitecture.Xtensa,
        "" => MachineArchitecture.Unknown,
        _ when !string.IsNullOrEmpty(arch) => MachineArchitecture.Custom,
        _ => MachineArchitecture.Unknown  // Sanity fallback
    };

    public static ushort GetElf(this MachineArchitecture arch) => arch switch
    {
        MachineArchitecture.X86 => 0x03,
        MachineArchitecture.X86_64 => 0x3E, 
        MachineArchitecture.IA64 => 0x32,
        MachineArchitecture.Arm or MachineArchitecture.ArmV7 => 0x28,
        MachineArchitecture.Arm64 => 0xB7,
        MachineArchitecture.PowerPc => 0x14,
        MachineArchitecture.PowerPc64 or MachineArchitecture.PowerPc64LE => 0x15,
        MachineArchitecture.S390 or MachineArchitecture.S390X => 0x16,
        MachineArchitecture.Mips or MachineArchitecture.Mips64 => 0x08,
        MachineArchitecture.Sparc => 0x02,
        MachineArchitecture.Sparc64 => 0x2B,
        MachineArchitecture.RiscV or MachineArchitecture.RiscV64 => 0xF3,
        MachineArchitecture.Alpha => 0x9026,
        MachineArchitecture.PaRisc => 0x0F,
        MachineArchitecture.Motorola68K => 0x04,
        MachineArchitecture.SuperH => 0x2A,
        MachineArchitecture.TilePro => 0xBC,
        MachineArchitecture.TileGx => 0xBF,
        MachineArchitecture.Cris => 0x4C,
        MachineArchitecture.Arc => 0x5D,
        MachineArchitecture.MicroBlaze => 0xBD,
        MachineArchitecture.Nios2 => 0x71,
        MachineArchitecture.Xtensa => 0x5E,
        _ => 0,
    };
}