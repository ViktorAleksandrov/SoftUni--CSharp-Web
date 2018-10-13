using System;
using System.Security.Cryptography;
using System.Text;
using IRunes.Services.Contracts;

namespace IRunes.Services
{
    public class HashService : IHashService
    {
        public string Hash(string stringToHash)
        {
            stringToHash = stringToHash + "IRunesSalt:!o2096mndgvk";

            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));

                string hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                return hash;
            }
        }
    }
}
