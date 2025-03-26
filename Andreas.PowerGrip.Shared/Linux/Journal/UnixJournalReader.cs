namespace Andreas.PowerGrip.Shared.Linux.Journal;

public class UnixJournalReader : IDisposable
{
    private IntPtr _journal;

    private const int SD_JOURNAL_LOCAL_ONLY = 1;

    [DllImport("libsystemd.so.0")]
    private static extern int sd_journal_open(out IntPtr journal, int flags);

    [DllImport("libsystemd.so.0")]
    private static extern int sd_journal_next(IntPtr journal);

    [DllImport("libsystemd.so.0")]
    private static extern int sd_journal_get_data(IntPtr journal, string field, out IntPtr data, out IntPtr length);

    [DllImport("libsystemd.so.0")]
    private static extern int sd_journal_add_match(IntPtr journal, string match, ulong matchLen);

    [DllImport("libsystemd.so.0")]
    private static extern void sd_journal_flush_matches(IntPtr journal);

    [DllImport("libsystemd.so.0")]
    private static extern void sd_journal_close(IntPtr journal);

    public UnixJournalReader()
    {
        if (sd_journal_open(out _journal, SD_JOURNAL_LOCAL_ONLY) < 0)
        {
            throw new Exception("Failed to open systemd journal.");
        }
    }

    public void SetFilter(JournalFilterType filterType, object value)
    {
        string? key = filterType.GetEnumMemberValue();
        if (key is null)
        {
            throw new ArgumentException("Invalid filter type.", nameof(filterType));
        }

        string match = $"{key}={value}";

        if (sd_journal_add_match(_journal, match, (ulong)match.Length) < 0)
        {
            throw new Exception("Failed to apply journal filter.");
        }
    }

    public void RemoveAllFilters()
    {
        // Reset all filters and reapply the remaining ones
        sd_journal_flush_matches(_journal);
    }

    public List<JournalLogEntry> ReadLogEntries()
    {
        List<JournalLogEntry> entries = new List<JournalLogEntry>();

        while (sd_journal_next(_journal) > 0)
        {
            string? message = GetFieldValue("MESSAGE");
            string? priorityStr = GetFieldValue("PRIORITY");
            string? timestampStr = GetFieldValue("__REALTIME_TIMESTAMP");

            // Parse log level
            JournalLogLevel level = JournalLogLevel.Info;
            if (!Enum.TryParse<JournalLogLevel>(priorityStr, out level))
            {
                level = JournalLogLevel.Info;
            }

            // Parse timestamp (microseconds since epoch)
            DateTimeOffset timestamp = DateTimeOffset.MinValue;
            if (long.TryParse(timestampStr, out long microseconds))
            {
                long milliseconds = microseconds / 1000;
                timestamp = DateTimeOffset.FromUnixTimeMilliseconds(milliseconds);
            }

            entries.Add(new JournalLogEntry
            {
                Timestamp = timestamp,
                Level = level,
                Message = message ?? string.Empty
            });
        }

        return entries;
    }

    private string? GetFieldValue(string field)
    {
        IntPtr data, length;
        if (sd_journal_get_data(_journal, field, out data, out length) == 0)
        {
            string output = Marshal.PtrToStringAnsi(data)!;
            string[] parts = output.Split(new[] { '=' }, 2);
            return parts.Length > 1 ? parts[1] : null;
        }
        return null;
    }

    public void Dispose()
    {
        if (_journal != IntPtr.Zero)
        {
            sd_journal_close(_journal);
            _journal = IntPtr.Zero;
        }
    }
}
