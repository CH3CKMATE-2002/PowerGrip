namespace Andreas.PowerGrip.Shared.Extensions;

public static class StringExtensions
{
    public static bool IsEmpty(this string s)
        => string.IsNullOrEmpty(s);
    
    public static bool IsWhiteSpace(this string s)
        => string.IsNullOrWhiteSpace(s);
    
    public static bool SameAs(this string left, string right)
        => left.Equals(right, StringComparison.OrdinalIgnoreCase);
    
    public static string ToUrlSafeString(this string s)
        => s.TrimEnd('=').Replace('+', '-').Replace('/', '_');
}