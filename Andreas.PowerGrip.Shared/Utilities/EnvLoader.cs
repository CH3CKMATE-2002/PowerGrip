namespace Andreas.PowerGrip.Shared.Utilities;

public static class EnvLoader
{
    public static Dictionary<string, string> Load(string filePath = ".env")
    {
        if (!File.Exists(filePath))
        {
            return [];
        }

        Dictionary<string, string> result = [];

        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#"))
            {
                // Entire line is a comment or empty.
                continue;
            }

            // Split key and value by the first '='
            var parts = line.Split('=', 2);
            if (parts.Length != 2)
            {
                // Malformed line, skip.
                continue;
            }

            var key = parts[0].Trim();
            var rawValue = parts[1].Trim();

            // Handle inline comments and quoted values
            var value = ParseValue(rawValue);
            
            result[key] = value;
        }

        return result;
    }

    private static string ParseValue(string rawValue)
    {
        bool inQuotes = false;
        char quoteChar = '\0';
        var sb = new StringBuilder();

        for (int i = 0; i < rawValue.Length; i++)
        {
            char c = rawValue[i];

            if (!inQuotes && (c == '#' || (c == '/' && i + 1 < rawValue.Length && rawValue[i + 1] == '/')))
            {
                // Start of inline comment outside quotes
                break;
            }

            if ((c == '"' || c == '\'') && (i == 0 || rawValue[i - 1] != '\\'))
            {
                if (!inQuotes)
                {
                    inQuotes = true;
                    quoteChar = c;
                }
                else if (quoteChar == c)
                {
                    inQuotes = false;
                }
                continue; // Skip quote chars
            }

            sb.Append(c);
        }

        return sb.ToString().Trim();
    }
}
