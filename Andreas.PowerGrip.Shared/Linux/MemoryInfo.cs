namespace Andreas.PowerGrip.Shared.Linux;

public class MemoryInfo
{
    public ulong TotalRamKb { get; set; }
    public ulong FreeRamKb { get; set; }
    public ulong AvailableRamKb { get; set; }
    public ulong CachedRamKb { get; set; }
    public ulong SwapCachedKb { get; set; }
    public ulong SwapTotalKb { get; set; }
    public ulong SwapFreeKb { get; set; }

    public static MemoryInfo ParseFromFile(string path)
    {
        // NOTE: The path supplied is usually '/proc/meminfo'
        var memInfo = new MemoryInfo();
        using var stream = new StreamReader(path);

        while (!stream.EndOfStream)
        {
            var line = stream.ReadLine()!;
            var splitted = line.Split(':', 2, StringSplitOptions.TrimEntries);
            
            if (splitted.Length < 2) return memInfo;

            var key = splitted[0];
            var value = splitted[1].Split()[0];

            switch (key)
            {
                case "MemTotal":
                    memInfo.TotalRamKb = ulong.Parse(value);
                    break;
                case "MemFree":
                    memInfo.FreeRamKb = ulong.Parse(value);
                    break;
                case "MemAvailable":
                    memInfo.AvailableRamKb = ulong.Parse(value);
                    break;
                case "Cached":
                    memInfo.CachedRamKb = ulong.Parse(value);
                    break;
                case "SwapCached":
                    memInfo.SwapCachedKb = ulong.Parse(value);
                    break;
                case "SwapTotal":
                    memInfo.SwapTotalKb = ulong.Parse(value);
                    break;
                case "SwapFree":
                    memInfo.SwapFreeKb = ulong.Parse(value);
                    break;
            }
        }

        return memInfo;
    }
}