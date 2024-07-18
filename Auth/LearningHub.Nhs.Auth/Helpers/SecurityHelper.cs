/*
  https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.rfc2898derivebytes?view=net-5.0
  Implements password-based key derivation functionality, PBKDF2, by using a pseudo-random number generator based on HMACSHA1.

  Alternatively Install Microsoft.AspNetCore.Cryptography.KeyDerivation
  var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(pwd, salt, KeyDerivationPrf.HMACSHA512, 10000, 32));
*/

namespace LearningHub.Nhs.Auth.Helpers
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// The security helper.
    /// </summary>
    public static class SecurityHelper
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Generate hash.
        /// </summary>
        /// <param name="state">The state value.</param>
        /// <param name="secretKey">The client secret.</param>
        /// <returns>A hash value.</returns>
        public static string GenerateHash(string state, string secretKey)
        {
            var secondsSinceEpoch = GetSecondsSinceEpoch();
            var encoder = new UTF8Encoding();
            var salt = encoder.GetBytes(secretKey);
            var pwd = encoder.GetBytes(state + secondsSinceEpoch);
            return GenerateHash(pwd, salt);
        }

        /// <summary>
        /// Verify hash.
        /// </summary>
        /// <param name="state">The state value.</param>
        /// <param name="secretKey">The client secret.</param>
        /// <param name="hash">Hash value to verify.</param>
        /// <returns>Indicating whether its a valid hash.</returns>
        public static bool VerifyHash(string state, string secretKey, string hash)
        {
            var secondsSinceEpoch = GetSecondsSinceEpoch();
            var encoder = new UTF8Encoding();
            var salt = encoder.GetBytes(secretKey);
            var toleranceInSec = 60;

            // loop optimisation, iterates 0,-1,-2,-3 .. -60,1,2,3 .. 60
            for (int counter = 0; counter <= toleranceInSec * 2; counter++)
            {
                var step = counter > toleranceInSec ? counter - toleranceInSec : -1 * counter;

                var pwd = encoder.GetBytes(state + (secondsSinceEpoch + step));

                if (hash == GenerateHash(pwd, salt))
                {
                    return true;
                }
            }

            return false;
        }

        private static string GenerateHash(byte[] password, byte[] salt)
        {
            using var byteResult = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA512);
            var hash = Convert.ToBase64String(byteResult.GetBytes(32));
            return hash;
        }

        private static long GetSecondsSinceEpoch()
        {
            return (long)(DateTime.UtcNow - UnixEpoch).TotalSeconds;
        }
    }
}