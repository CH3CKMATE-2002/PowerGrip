namespace Andreas.PowerGrip.Shared.Utilities;
public static class Guard
{
    public static T ReturnOrThrowIfNull<T>(T? data)
        => data ?? throw new NullReferenceException("The given argument was null");
    
    public static void ThrowIfOutOfRange<T>(T value, T min, T max) where T: IComparisonOperators<T, T, bool>
    {
        if (!value.IsBetween(min, max))
        {
            throw new ArgumentOutOfRangeException(
                paramName: nameof(value),
                actualValue: value,
                message: $"{nameof(value)} must be between {min} and {max}.");
        }
    }

    public static void Assert(bool test, string? message = null)
    {
        if (test) return;

        throw new InvalidOperationException(message ?? "The assertion has failed");
    }
}