using System.Security.Cryptography;
using System.Text;

namespace TimeBooker.Model.Helpers;

public static class PasswordHelper
{
    public static string Hash(this string password)
    {
        using (var sha256Hash = SHA256.Create())
        {
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            var builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
                builder.Append(bytes[i].ToString("x2"));

            return builder.ToString();
        }
    }
}