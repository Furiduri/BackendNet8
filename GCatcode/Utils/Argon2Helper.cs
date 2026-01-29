using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace GCatcode.Utils
{
    public static class Argon2Helper
    {
        private const int DegreeOfParallelism = 8;
        private const int Iterations = 4;
        private const int MemorySize = 65536; // 64MB
        private const int SaltSize = 16;
        private const int HashSize = 32;

        public static string HashPassword(string password)
        {
            var salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = DegreeOfParallelism,
                Iterations = Iterations,
                MemorySize = MemorySize
            };

            var hash = argon2.GetBytes(HashSize);

            // Combine salt and hash for storage
            var combinedBytes = new byte[SaltSize + HashSize];
            Buffer.BlockCopy(salt, 0, combinedBytes, 0, SaltSize);
            Buffer.BlockCopy(hash, 0, combinedBytes, SaltSize, HashSize);

            return Convert.ToBase64String(combinedBytes);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                var combinedBytes = Convert.FromBase64String(hashedPassword);
                //if (combinedBytes.Length != SaltSize + HashSize)
                //{
                //    return false;
                //}

                var salt = new byte[SaltSize];
                var expectedHash = new byte[HashSize];
                Buffer.BlockCopy(combinedBytes, 0, salt, 0, SaltSize);
                Buffer.BlockCopy(combinedBytes, SaltSize, expectedHash, 0, HashSize);

                var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
                {
                    Salt = salt,
                    DegreeOfParallelism = DegreeOfParallelism,
                    Iterations = Iterations,
                    MemorySize = MemorySize
                };

                var actualHash = argon2.GetBytes(HashSize);

                // Constant-time comparison
                return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
            }
            catch
            {
                return false;
            }
        }
    }
}
