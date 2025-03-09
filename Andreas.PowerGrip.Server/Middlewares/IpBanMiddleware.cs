namespace Andreas.PowerGrip.Server.Middlewares;

[Obsolete("Kept for the record and was used in early stages of the app; Rate-limiting logic was moved to Handshake logic instead")]
public record BanRecord(int AttemptCount, DateTime BanTime)
{
    public static BanRecord Default => new(0, DateTime.MinValue);
}

[Obsolete("Kept for the record and was used in early stages of the app; Rate-limiting logic was moved to Handshake logic instead")]
public class IpBanMiddleware : BaseMiddleware
{
    private static readonly ConcurrentDictionary<string, BanRecord> _ipAttempts = new();
    private readonly int _maxAttempts = 5;
    private readonly TimeSpan _banDuration = TimeSpan.FromMinutes(10);
    private static readonly TimeSpan CleanupInterval = TimeSpan.FromMinutes(15);

    public IpBanMiddleware(RequestDelegate next) : base(next)
    {
        // Start periodic cleanup task
        Task.Run(PeriodicCleanupTask);
    }

    protected override async Task BeforeRequestAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
            return; // Authenticated users bypass the limit

        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        if (string.IsNullOrEmpty(ipAddress)) return;

        var now = DateTime.UtcNow;
        var record = _ipAttempts.GetValueOrDefault(ipAddress, BanRecord.Default);

        // Check if user is currently banned
        if (record.BanTime > now)
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            await context.Response.WriteAsync($"Too many requests. You are banned until {record.BanTime:HH:mm:ss} UTC.");
            return;
        }

        // Reset attempts if previous ban expired
        if (record.BanTime <= now && record.BanTime != DateTime.MinValue)
        {
            record = BanRecord.Default;
        }

        // Increment attempt count
        var newRecord = new BanRecord(record.AttemptCount + 1, now);
        _ipAttempts[ipAddress] = newRecord;

        // Check if attempts exceed limit
        if (newRecord.AttemptCount >= _maxAttempts)
        {
            _ipAttempts[ipAddress] = new BanRecord(newRecord.AttemptCount, now.Add(_banDuration));
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            await context.Response.WriteAsync($"Too many requests. Your IP is now temporarily banned until {now.Add(_banDuration):HH:mm:ss} UTC.");
            return;
        }
    }

    private async Task PeriodicCleanupTask()
    {
        while (true)
        {
            await Task.Delay(CleanupInterval);

            var now = DateTime.UtcNow;
            foreach (var key in _ipAttempts.Keys)
            {
                if (_ipAttempts.TryGetValue(key, out var record) && record.BanTime < now)
                {
                    _ipAttempts.TryRemove(key, out _);
                }
            }
        }
    }
}

