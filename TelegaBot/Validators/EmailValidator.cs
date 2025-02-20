using System.Text.RegularExpressions;

namespace TelegaBot.Validators;

public class EmailValidator
{
    private static readonly string EmailPattern =
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

    
    public static bool IsValid(string email)
    {
        return !string.IsNullOrWhiteSpace(email) && Regex.IsMatch(email, EmailPattern);
    }
}