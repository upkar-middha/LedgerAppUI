using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
namespace LedgerAppUI.Helper

{
    class HashStringUsingSHA
    {
        public static string Hash(string s)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(s));
            string hashedPassword = Convert.ToBase64String(hashBytes);
            return hashedPassword;
        }
    }
}
