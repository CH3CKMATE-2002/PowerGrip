namespace Andreas.PowerGrip.Shared.Linux.Journal;

public class JournalLogEntry
{
    public string Message { get; set; } = string.Empty;

    public JournalLogLevel Level { get; set; }
    
    public DateTimeOffset Timestamp { get; set; }


    public void Deconstruct(
        out string message,
        out JournalLogLevel level,
        out DateTimeOffset timestamp)
    {
        message = Message;
        level = Level;
        timestamp = Timestamp;
    }
}