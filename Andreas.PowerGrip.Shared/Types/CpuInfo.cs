namespace Andreas.PowerGrip.Shared.Types;

public class CpuInfo
{
    /// <summary>
    /// The logical processor number (zero-based index).
    /// </summary>
    public int ProcessorNumber { get; set; }

    /// <summary>
    /// The CPU vendor, e.g., "GenuineIntel" or "AuthenticAMD".
    /// </summary>
    public string VendorId { get; set; } = string.Empty;

    /// <summary>
    /// The CPU family number. Identifies the processor family (e.g., 6 for modern Intel CPUs).
    /// </summary>
    public int CpuFamily { get; set; }

    /// <summary>
    /// The model number within the CPU family. Further differentiates processors of the same family.
    /// </summary>
    public int ModelNumber { get; set; }

    /// <summary>
    /// The full model name of the CPU (e.g., "Intel(R) Core(TM) i7-4500U CPU @ 1.80GHz").
    /// </summary>
    public string ModelName { get; set; } = string.Empty;

    /// <summary>
    /// The stepping number, which indicates minor revisions of the CPU design.
    /// </summary>
    public int Stepping { get; set; }

    /// <summary>
    /// The microcode version running on the CPU, used for bug fixes and optimizations.
    /// </summary>
    public int Microcode { get; set; }

    /// <summary>
    /// The current clock speed of the CPU core in MHz.
    /// </summary>
    public float CurrentMegaHertz { get; set; }

    /// <summary>
    /// The size of the L2 cache (in KB).
    /// </summary>
    public int CacheSizeKB { get; set; }

    /// <summary>
    /// The physical ID of the CPU (used to differentiate multiple physical processors in a system).
    /// </summary>
    public int PhysicalId { get; set; }

    /// <summary>
    /// The total number of logical processors (including Hyper-Threading if available) in the same physical package.
    /// </summary>
    public int Siblings { get; set; }

    /// <summary>
    /// The core ID, differentiating physical cores within the same CPU package.
    /// </summary>
    public int CoreId { get; set; }

    /// <summary>
    /// The number of physical cores per CPU package.
    /// </summary>
    public int Cores { get; set; }

    /// <summary>
    /// The CPU's APIC (Advanced Programmable Interrupt Controller) ID, used for CPU identification in multiprocessor systems.
    /// </summary>
    public int ApicId { get; set; }

    /// <summary>
    /// The initial APIC ID before any BIOS or firmware modifications.
    /// </summary>
    public int InitialApicId { get; set; }

    /// <summary>
    /// Indicates whether the Floating Point Unit (FPU) is present.
    /// </summary>
    public bool Fpu { get; set; }

    /// <summary>
    /// Indicates whether the CPU supports FPU exceptions.
    /// </summary>
    public bool FpuException { get; set; }

    /// <summary>
    /// The CPU-ID instruction level supported by the processor.
    /// </summary>
    public int CpuIdLevel { get; set; }

    /// <summary>
    /// Write-Protect flag. Determines if the CPU enforces write protection at supervisor level.
    /// </summary>
    public bool WriteProtect { get; set; }

    /// <summary>
    /// A list of supported CPU flags, which define various features like virtualization, SIMD instructions, and security enhancements.
    /// </summary>
    public string[] Flags { get; set; } = [];

    /// <summary>
    /// A list of VMX (Virtual Machine Extensions) flags, indicating support for virtualization features.
    /// </summary>
    public string[] VmxFlags { get; set; } = [];

    /// <summary>
    /// A list of known CPU bugs or vulnerabilities (e.g., Spectre, Meltdown).
    /// </summary>
    public string[] Bugs { get; set; } = [];

    /// <summary>
    /// A rough estimation of CPU performance, calculated using BogoMIPS (Bogus Millions of Instructions Per Second).
    /// </summary>
    public float BogoMips { get; set; }

    /// <summary>
    /// Cache line flush size in bytes, used for memory performance optimizations.
    /// </summary>
    public int CacheLineFlushSize { get; set; }

    /// <summary>
    /// Cache alignment size in bytes, indicating the optimal memory alignment for performance.
    /// </summary>
    public int CacheAlignment { get; set; }

    /// <summary>
    /// The physical address size in bits (determines max supported RAM).
    /// </summary>
    public int PhysicalAddressSize { get; set; }

    /// <summary>
    /// The virtual address size in bits (determines max virtual memory addressing).
    /// </summary>
    public int VirtualAddressSize { get; set; }

    /// <summary>
    /// Power management profile, showing power-saving features supported by the CPU.
    /// </summary>
    public string PowerManagementProfile { get; set; } = string.Empty;

    /// <summary>
    /// Parses CPU information from a given file, typically "/proc/cpuinfo".
    /// </summary>
    /// <param name="path">The path to the file containing CPU information.</param>
    /// <returns>An array of <see cref="CpuInfo"/> objects, each representing a logical processor.</returns>
    /// <remarks>
    /// This method reads the file line by line, extracting key-value pairs from "/proc/cpuinfo".  
    /// If the file does not exist, it returns an empty array.  
    /// Currently, only a subset of attributes is processed, and further parsing logic may be needed  
    /// to fully populate all properties.
    /// </remarks>
    public static CpuInfo[] ParseFromFile(string path)
    {
        // NOTE: The input is almost always going to be '/proc/cpuinfo'
        List<CpuInfo> cpus = [];

        if (!File.Exists(path))
        {
            return cpus.ToArray();  // Welp, nuthin' to do!
        }

        var content = File.ReadAllLines(path);
        CpuInfo? currentCpu = null;

        foreach (var line in content)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                // End of a CPU entry, store the current CPU
                if (currentCpu != null)
                {
                    cpus.Add(currentCpu);
                    currentCpu = null;
                }
                continue;
            }

            var splitted = line.Split(":", 2, StringSplitOptions.TrimEntries);
            if (splitted.Length < 2) continue;

            var key = splitted[0];
            var value = splitted[1];

            if (key == "processor")
            {
                // Start a new CPU entry
                if (currentCpu != null) cpus.Add(currentCpu); // Store the previous CPU
                currentCpu = new CpuInfo();
                currentCpu.ProcessorNumber = int.Parse(value);
                continue;
            }

            if (currentCpu == null) continue; // Safety check

            switch (key)
            {
                case "vendor_id":
                    currentCpu.VendorId = value;
                    break;
                case "cpu family":
                    currentCpu.CpuFamily = int.Parse(value);
                    break;
                case "model":
                    currentCpu.ModelNumber = int.Parse(value);
                    break;
                case "model name":
                    currentCpu.ModelName = value;
                    break;
                case "stepping":
                    currentCpu.ModelNumber = int.Parse(value);
                    break;
                case "microcode":
                    currentCpu.Microcode = int.Parse(value);
                    break;
                case "cpu MHz":
                    currentCpu.CurrentMegaHertz = float.Parse(value);
                    break;
                case "cache size":
                    currentCpu.CacheSizeKB = int.Parse(value.Split(" ")[0]);
                    break;
                case "physical id":
                    currentCpu.PhysicalId = int.Parse(value);
                    break;
                case "siblings":
                    currentCpu.Siblings = int.Parse(value);
                    break;
                case "core id":
                    currentCpu.CoreId = int.Parse(value);
                    break;
                case "cpu cores":
                    currentCpu.Cores = int.Parse(value);
                    break;
                case "flags":
                    currentCpu.Flags = value.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    break;
                case "bugs":
                    currentCpu.Bugs = value.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    break;
                case "bogomips":
                    currentCpu.BogoMips = float.Parse(value);
                    break;
                case "clflush size":
                    currentCpu.CacheLineFlushSize = int.Parse(value);
                    break;
                case "cache_alignment":
                    currentCpu.CacheAlignment = int.Parse(value);
                    break;
                case "address sizes":
                    var sizes = value.Split(",", StringSplitOptions.TrimEntries);
                    if (sizes.Length >= 2)
                    {
                        currentCpu.PhysicalAddressSize = int.Parse(sizes[0].Split(" ")[0]);
                        currentCpu.VirtualAddressSize = int.Parse(sizes[1].Split(" ")[0]);
                    }
                    break;
                case "power management":
                    currentCpu.PowerManagementProfile = value;
                    break;
            }
        }

        // Add the last CPU entry (if any)
        if (currentCpu != null) cpus.Add(currentCpu);

        return cpus.ToArray();
    }
}