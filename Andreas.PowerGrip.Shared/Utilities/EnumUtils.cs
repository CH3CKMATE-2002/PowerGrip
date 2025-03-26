namespace Andreas.PowerGrip.Shared.Utilities;

public static class EnumUtils
{
    public static string GetEnumMemberValue<T>(this T value, string? defaultValue = null) where T : Enum
    {
        var type = typeof(T);
        var name = Enum.GetName(type, value)!;
        var field = type.GetField(name);
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>();
        return attribute?.Value ?? defaultValue ?? value.ToString();
    }

    public static T ParseEnumMemberValue<T>(string value, T? defaultValue = default) where T : Enum
    {
        foreach (var field in typeof(T).GetFields())
        {
            if (!field.IsStatic) continue;
            
            var attribute = field.GetCustomAttribute<EnumMemberAttribute>();
            if (attribute?.Value == value)
            {
                return (T)field.GetValue(null)!;
            }
        }

        if (defaultValue is not null)
        {
            return defaultValue;
        }
        
        throw new ArgumentException($"Value '{value}' is not valid for enum {typeof(T).Name}");
    }
}