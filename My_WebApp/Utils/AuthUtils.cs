using System.Security.Cryptography;
using System.Text;

namespace My_WebApp.Utils
{
    public class AuthUtils
    {
        public static string HashPassword (string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public static bool VerifyPassword(string password, string hashedPassowrd)
        {
            var hashedPasswordBytes = Convert.FromBase64String(hashedPassowrd);
            using (var sha256 = SHA256.Create())
            {
                var computedHashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes (password));
                return hashedPasswordBytes.SequenceEqual(computedHashBytes);
            }
        }

    }
}
