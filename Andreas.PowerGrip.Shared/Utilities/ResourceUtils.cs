namespace Andreas.PowerGrip.Shared.Utilities;

public static class ResourceUtils
{
    public static string[]? ResourceNames()
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        if (assembly is null) return null;

        return assembly.GetManifestResourceNames();
    }

    public static Stream? ResourceStream(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();

        if (assembly is null) return null;

        try
        {
            return assembly.GetManifestResourceStream(name);
        }
        catch
        {
            return null;
        }
    }

    public static string? ResourceText(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        if (assembly is null) return null;

        using var stream = assembly.GetManifestResourceStream(name);

        if (stream is null) return null;

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public static async Task<string?> ReadTextAsync(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        if (assembly is null) return null;

        using var stream = assembly.GetManifestResourceStream(name);

        if (stream is null) return null;

        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}