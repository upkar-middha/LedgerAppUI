using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Collections.Specialized;
namespace LedgerAppUI.Services
{
    public class Authentication // make internal
    {

        public static void First_Login(string userPass, string userName, string MobileNumber)
        {
            //check if username already exists ?

            if (UserExists(userName))
                throw new Exception("Username already exists. Please choose a different username."); // or use a more specific exception type)

            //if(string.IsNullOrEmpty(userPass)) throw 

            // ----------------   default path

            string bahi_folder = AppPaths.BahiFolderPath;
            Directory.CreateDirectory(bahi_folder);

            string UserPath = AppPaths.UserDataPath;

            Directory.CreateDirectory(UserPath);

            string dbpath = AppPaths.userDbpath;

            // ----------------   default path ends here

            Database db = new(dbpath);
            string Create_cmd = "CREATE TABLE IF NOT EXISTS Users (\r\n    username VARCHAR(30) PRIMARY KEY,\r\n    hashedpass CHAR(44) NOT NULL,\r\n    salt CHAR(16) NOT NULL,\r\n    phone CHAR(10)\r\n);";
            db.createNewDB(Create_cmd);

            // hashing password 
            
            char pepper = (char)('a' + Random.Shared.Next(26));

            string salt = Generate_Salt();

            string hashed_pass = Hash_using_SHA(salt + userPass + pepper);

            // hashing password ends here


            string insert_cmd = "INSERT INTO Users (username, hashedpass, salt, phone)\r\nVALUES (@username, @hashedpass, @salt, @phone);\r\n";

            db.push_to_DB(insert_cmd, cmd => {
                cmd.Parameters.AddWithValue("@username", userName);
                cmd.Parameters.AddWithValue("@hashedpass", hashed_pass);
                cmd.Parameters.AddWithValue("@salt", salt);
                cmd.Parameters.AddWithValue("@phone", MobileNumber);
            });

        }

        public static void Change_password(string username , string new_password)
        {
            string salt = Generate_Salt();
            char pepper = (char)('a' + Random.Shared.Next(26));
            string hashed_pass = Hash_using_SHA(salt + new_password + pepper);
            string dbpath = AppPaths.userDbpath;
        
            // Check if the database file exists
            if (!File.Exists(dbpath))
            {
                throw new FileNotFoundException("Database file not found.", dbpath);
            }

            Database db = new(dbpath);

            // check if new_password is different from the old password
            if (Validate_User(username, new_password))
            {
                throw new ArgumentException("New password cannot be the same as the old password.");
            }
            string cmd = "UPDATE Users\r\nSET hashedpass = @hashed_pass, salt = @salt\r\nWHERE username = @username;\r\n";

            db.Update_DB(cmd, parameter => {
                parameter.Parameters.AddWithValue("@hashed_pass", hashed_pass);
                parameter.Parameters.AddWithValue("@salt", salt);
                parameter.Parameters.AddWithValue("@username", username);
            }
            );
        }

        public static void change_mobile(string username, string new_mobile)
        {
            string dbpath = AppPaths.userDbpath;

            // Check if the database file exists
            if (!File.Exists(dbpath))
            {
                throw new FileNotFoundException("Database file not found.", dbpath);
            }

            Database db = new(dbpath);

            string cmd = "UPDATE Users\r\nSET phone = @phone\r\nWHERE username = @username;\r\n";

            db.Update_DB(cmd, parameter =>
            {
                parameter.Parameters.AddWithValue("@phone", new_mobile);
                parameter.Parameters.AddWithValue("@username", username);
            });
        }

        private static string Hash_using_SHA(string s) // make private
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(s));
            string hashedPassword = Convert.ToBase64String(hashBytes);
            return hashedPassword;
        }

        private static string Generate_Salt()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            string salt = new(Enumerable.Repeat(chars, 10).Select(s => s[Random.Shared.Next(s.Length)]).ToArray());
            return salt;
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
        private static bool UserExists(string userName)
        {
            if (!File.Exists(AppPaths.userDbpath))
            {
                return false;
            }
            string dbpath = AppPaths.userDbpath;
            Database db = new(dbpath);

            string command = "SELECT * FROM Users u\r\nWHERE u.username = @username;\r\n";

            List<Dictionary<string, object>> rows = db.Fetch_from_DB(command, cmd =>
            {
                cmd.Parameters.AddWithValue("username", userName);
            });
            Console.WriteLine($"Rows found: {rows.Count}");
            return rows.Count > 0;
        }

    }
}
