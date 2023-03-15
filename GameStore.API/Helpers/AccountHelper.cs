using GameStore.Domain.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace GameStore.API.Helpers
{
    public static class AccountHelper
    {
        public static bool CheckCorrectPassword(User user, string password, string salt)
        {
            var hash = HashPassword(password, salt);
            if (!user.Password.Equals(hash))
            {
                return false;
            }

            return true;
        }

        public static string HashPassword(string password, string salt)
        {
            const int keySize = 64;
            const int iterationCount = 1000;
            
            var utf8 = new UTF8Encoding();
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
