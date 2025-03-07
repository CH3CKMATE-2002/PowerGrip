namespace Andreas.PowerGrip.Shared.Extensions;

public static class CharacterExtensions
{
    public static bool IsLetter(this char c)
        => char.IsLetter(c);  // c is >= 'a' and <= 'z' || c is >= 'A' and <= 'z'
    
    public static bool IsDigit(this char c)
        => char.IsDigit(c);  // c is >= '0' and <= '9'
    
    public static bool IsSymbol(this char c)
        => char.IsSymbol(c);
}