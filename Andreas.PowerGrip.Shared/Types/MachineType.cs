namespace Andreas.PowerGrip.Shared.Types;

/// <summary>
/// Represents the architecture or type of a machine.
/// </summary>
public enum MachineType
{
    /// <summary>
    /// Unknown or unspecified machine type.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// 32-bit x86 architecture (Intel 80386 and later).
    /// Commonly used in older PCs and embedded systems.
    /// Raw value: 0x014C (IMAGE_FILE_MACHINE_I386).
    /// </summary>
    I386 = 0x014C,

    /// <summary>
    /// 64-bit x86 architecture (AMD64 or Intel x64).
    /// Commonly used in modern PCs and servers.
    /// Raw value: 0x8664 (IMAGE_FILE_MACHINE_AMD64).
    /// </summary>
    X86_64 = 0x8664,

    /// <summary>
    /// ARM architecture (32-bit).
    /// Commonly used in mobile devices and embedded systems.
    /// Raw value: 0x01C2 (IMAGE_FILE_MACHINE_ARM).
    /// </summary>
    ARM = 0x01C2,

    /// <summary>
    /// ARM architecture (64-bit).
    /// Commonly used in modern mobile devices and servers.
    /// Raw value: 0xAA64 (IMAGE_FILE_MACHINE_ARM64).
    /// </summary>
    ARM64 = 0xAA64,

    /// <summary>
    /// Intel Itanium architecture (64-bit).
    /// Used in high-performance servers.
    /// Raw value: 0x0200 (IMAGE_FILE_MACHINE_IA64).
    /// </summary>
    IA64 = 0x0200,

    /// <summary>
    /// MIPS architecture (32-bit).
    /// Commonly used in embedded systems and older workstations.
    /// Raw value: 0x0166 (IMAGE_FILE_MACHINE_MIPS16).
    /// </summary>
    MIPS = 0x0166,

    /// <summary>
    /// MIPS architecture (64-bit).
    /// Used in some high-performance embedded systems.
    /// Raw value: 0x0168 (IMAGE_FILE_MACHINE_MIPSFPU).
    /// </summary>
    MIPS64 = 0x0168,

    /// <summary>
    /// PowerPC architecture (32-bit).
    /// Commonly used in older Apple Macintosh systems and embedded devices.
    /// Raw value: 0x01F2 (IMAGE_FILE_MACHINE_POWERPC).
    /// </summary>
    PowerPC = 0x01F2,

    /// <summary>
    /// PowerPC architecture (64-bit).
    /// Used in some high-performance servers and workstations.
    /// Raw value: 0x01F6 (IMAGE_FILE_MACHINE_POWERPCFPU).
    /// </summary>
    PowerPC64 = 0x01F6,

    /// <summary>
    /// SPARC architecture (32-bit).
    /// Commonly used in older Sun Microsystems workstations and servers.
    /// Raw value: 0x0162 (IMAGE_FILE_MACHINE_SPARC).
    /// </summary>
    SPARC = 0x0162,

    /// <summary>
    /// SPARC architecture (64-bit).
    /// Used in high-performance servers.
    /// Raw value: 0x016B (IMAGE_FILE_MACHINE_SPARCV9).
    /// </summary>
    SPARC64 = 0x016B,

    /// <summary>
    /// RISC-V architecture (32-bit).
    /// An open-source instruction set architecture used in embedded systems.
    /// Raw value: 0x5032 (custom value for RISC-V 32-bit).
    /// </summary>
    RISCV32 = 0x5032,

    /// <summary>
    /// RISC-V architecture (64-bit).
    /// An open-source instruction set architecture used in high-performance systems.
    /// Raw value: 0x5064 (custom value for RISC-V 64-bit).
    /// </summary>
    RISCV64 = 0x5064,

    /// <summary>
    /// s390x architecture (64-bit).
    /// Used in IBM mainframe systems.
    /// Raw value: 0x16DC (IMAGE_FILE_MACHINE_S390X).
    /// </summary>
    S390X = 0x16DC
}