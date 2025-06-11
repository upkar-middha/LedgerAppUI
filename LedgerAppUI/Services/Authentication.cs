using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
namespace LedgerAppUI.Services
{
    public class Authentication // make internal
    {

        public static void First_Login(string userPass, string userName, string MobileNumber)
        {
            //if(string.IsNullOrEmpty(userPass)) throw 
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            string salt = new(Enumerable.Repeat(chars, 10).Select(s => s[Random.Shared.Next(s.Length)]).ToArray());
            char pepper = (char)('a' + Random.Shared.Next(26));

            string hashed_pass = Hash_using_SHA(salt + userPass + pepper);

            // ----------------   default path
            string bahi_folder = AppPaths.BahiFolderPath;

            Directory.CreateDirectory(bahi_folder);

            string UserPath = AppPaths.UserDataPath;

            Directory.CreateDirectory(UserPath);

            string dbpath = AppPaths.userDbpath;

            // ----------------   default path

            Database db = new(dbpath);

            string Create_cmd = "CREATE TABLE IF NOT EXISTS User (\r\n    username VARCHAR(30) PRIMARY KEY,\r\n    hashedpass CHAR(44) NOT NULL,\r\n    salt CHAR(16) NOT NULL,\r\n    phone CHAR(10)\r\n);";
            db.createNewDB(Create_cmd);

            string insert_cmd = "INSERT INTO Users (username, hashedpass, salt, phone)\r\nVALUES (@username, @hashedpass, @salt, @phone);\r\n";

            db.push_to_DB(insert_cmd, cmd => {
                cmd.Parameters.AddWithValue("@username", userName);
                cmd.Parameters.AddWithValue("@hashedpass", hashed_pass);
                cmd.Parameters.AddWithValue("@salt", salt);
                cmd.Parameters.AddWithValue("@phone", MobileNumber);
            });

        }

        public static string Hash_using_SHA(string s) // make private
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(s));
            string hashedPassword = Convert.ToBase64String(hashBytes);
            return hashedPassword;
        }

        public static bool Validate_User(string userName , string userPass)
        {
            string dbpath = AppPaths.userDbpath;
            Database db = new(dbpath);

            string command = "SELECT * FROM Users u\r\nWHERE u.username = @username;\r\n";

            List<Dictionary<string, object>> rows = db.Fetch_from_DB(command , cmd => {
                cmd.Parameters.AddWithValue("username", userName);
            });

            string salt = (string)rows[0]["salt"];
            string hashedpass = (string)rows[0]["hashedpass"];

            for(int i = 0; i < 26; i++)
            {
                if (Hash_using_SHA(salt + userPass + (char)('a' + i)) == hashedpass) return true;
            }
            return false;
        }

    }
}
