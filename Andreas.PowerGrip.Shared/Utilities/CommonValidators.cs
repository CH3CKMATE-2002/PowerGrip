namespace Andreas.PowerGrip.Shared.Utilities;

public static class CommonValidators
{
    public static bool IsValidEmail(string email)
    {
        try
        {
            var mail = new MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
}