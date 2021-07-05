using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Warehouse.Service.Abstractions;

namespace Warehouse.Service
{
    public class HashPasswordService : IHashPasswordService
    {
        public const int SaltByteSize = 24;
        public const int HashByteSize = 24;
        public const int Pbkdf2Iterations = 1000;

        public const int IterationIndex = 0;
        public const int SaltIndex = 1;
        public const int Pbkdf2Index = 2;
        
       

        public string Hash(string password)
        {
            // Generate a random salt
            var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var salt = new byte[SaltByteSize];
            rngCryptoServiceProvider.GetBytes(salt);

            // Hash the password and encode the parameters
            var hash = Pbkdf2(password, salt, Pbkdf2Iterations, HashByteSize);
            return Pbkdf2Iterations + ":" +
                   Convert.ToBase64String(salt) + ":" +
                   Convert.ToBase64String(hash);
        }

        private static byte[] Pbkdf2(string password, byte[] salt, int iterations, int outputBytes)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt) {IterationCount = iterations};
            return pbkdf2.GetBytes(outputBytes);
        }

        public bool Verify(string password, string hashedPassword)
        {
            // Extract the parameters from the hash
            char[] delimiter = { ':' };
            var split = hashedPassword.Split(delimiter);
            var iterations = Int32.Parse(split[IterationIndex]);
            var salt = Convert.FromBase64String(split[SaltIndex]);
            var hash = Convert.FromBase64String(split[Pbkdf2Index]);

            var testHash = Pbkdf2(password, salt, iterations, hash.Length);
            return SlowEquals(hash, testHash);
        }


        private static bool SlowEquals(byte[] a, IReadOnlyList<byte> b)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            var diff = (uint)a.Length ^ (uint)b.Count;
            for (var i = 0; i < a.Length && i < b.Count; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }

            return diff == 0;
        }
    }
}