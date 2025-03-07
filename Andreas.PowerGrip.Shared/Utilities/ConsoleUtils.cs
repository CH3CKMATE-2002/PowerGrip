namespace Andreas.PowerGrip.Shared.Utilities;

public static class ConsoleUtils
{
    public static string ReadPassword(char echo = '\0')
    {
        StringBuilder password = new StringBuilder();
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);
            
            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
            {
                password.Append(key.KeyChar);
                if (echo != '\0') Console.Write(echo);
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password.Remove(password.Length - 1, 1);
                if (echo != '\0') Console.Write("\b \b");
            }
        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine();

        return password.ToString();
    }
}