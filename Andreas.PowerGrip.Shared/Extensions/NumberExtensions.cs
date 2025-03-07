namespace Andreas.PowerGrip.Shared.Extensions;

public static class NumberExtensions
{
    public static bool IsBetween<T>(this T value, T min, T max)
    where T: IComparisonOperators<T, T, bool>
        => value >= min && value <= max;
}