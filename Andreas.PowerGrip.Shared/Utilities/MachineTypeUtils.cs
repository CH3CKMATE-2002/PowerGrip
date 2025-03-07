namespace Andreas.PowerGrip.Shared.Utilities;

public static class MachineTypeUtils
{
    /// <summary>
    /// Parses the output of `uname -m` and returns the corresponding MachineType.
    /// </summary>
    /// <param name="unameOutput">The output of `uname -m`.</param>
    /// <returns>The corresponding MachineType, or MachineType.Unknown if no match is found.</returns>
    public static MachineType FromString(string machine)
    {
        return machine.ToLower() switch
        {
            "i386" or "i686" => MachineType.I386,
            "x86_64" or "amd64" => MachineType.X86_64,
            "armv7l" or "arm" => MachineType.ARM,
            "aarch64" => MachineType.ARM64,
            "ia64" => MachineType.IA64,
            "mips" or "mipsel" => MachineType.MIPS,
            "mips64" or "mips64el" => MachineType.MIPS64,
            "ppc" or "powerpc" => MachineType.PowerPC,
            "ppc64" or "ppc64le" => MachineType.PowerPC64,
            "sparc" => MachineType.SPARC,
            "sparc64" => MachineType.SPARC64,
            "riscv32" => MachineType.RISCV32,
            "riscv64" => MachineType.RISCV64,
            "s390x" => MachineType.S390X,
            _ => MachineType.Unknown
        };
    }
}