using GameStore.Domain.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace GameStore.API.Helpers
{
    static public class AccountHelper
    {
        static public bool CheckCorrectPassword(User user, string password)
        {
            var salt = password + user.Login;
            var hash = HashPassword(password, salt);
            if (!user.Password.Equals(hash))
            {
                return false;
            }

            return true;
        }

        static public string HashPassword(string password, string salt)
        {
            var utf8 = new UTF8Encoding();

            var keySize = 64;
            var iterationCount = 1000;
            var saltBytes = utf8.GetBytes(salt);

            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                saltBytes,
                KeyDerivationPrf.HMACSHA256,
                iterationCount,
                keySize
            ));

            return hash;
        }
    }
}
