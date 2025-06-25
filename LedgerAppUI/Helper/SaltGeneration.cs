using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedgerAppUI.Helper
{
    class SaltGeneration
    {
        public static string Salt(int size)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            string salt = new(Enumerable.Repeat(chars, size).Select(s => s[Random.Shared.Next(s.Length)]).ToArray());
            return salt;
        }
    }
}
