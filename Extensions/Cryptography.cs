using System.Security.Cryptography;
using System.Text;

namespace DotnetAPI.Extensions;

public static class Extensions
{
    public static string HashPassword(this string password)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(password);
        byte[] hash = SHA256.HashData(buffer);
        return Convert.ToBase64String(hash);
    }

    private static readonly Dictionary<char, string> specialCharacters = new()
    {
        { 'á', "&aacute" },
        { 'é', "&eacute" },
        { 'í', "&iacute" },
        { 'ó', "&oacute" },
        { 'ú', "&uacute" },
    };

    public static string CleanString(this string text)
    {
        string result = "";
        foreach (char c in text)
        {
            if (specialCharacters.TryGetValue(c, out string? s) && s is not null)
            {
                result += s;
                continue;
            }
            result += c;
        }

        return result; 
    }
}








