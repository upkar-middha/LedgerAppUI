using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedgerAppUI.Services;
using System.IO;
using LedgerAppUI.Helper;
namespace LedgerAppUI.Model
{
    class Authentication
    {
        public static bool Login(string username , string password)
        {
            Database db = new (AppPaths.userDbpath);

            string command = "SELECT * FROM Users u\r\nWHERE u.username = @username;\r\n";

            List<Dictionary<string, object>> rows = db.Fetch_from_DB(command, cmd => {
                cmd.Parameters.AddWithValue("username", username);
            });

            string salt = (string)rows[0]["salt"];
            string hashedpass = (string)rows[0]["hashedpass"];

            for (int i = 0; i < 26; i++)
            {
                if (HashStringUsingSHA.Hash(salt + password + (char)('a' + i)) == hashedpass) return true;
            }

            return false;
        }

        public static bool New_User(string username , string password , string mobile)
        {

            if(UserExists(username)) return false;

            Database db = new (AppPaths.userDbpath);

            string insert_cmd = "INSERT INTO Users (username, hashedpass, salt, phone)\r\nVALUES (@username, @hashedpass, @salt, @phone);\r\n";

            char pepper = (char)('a' + Random.Shared.Next(26));

            string salt = SaltGeneration.Salt(10);

            string hashedpass = HashStringUsingSHA.Hash(salt + password + pepper);

            db.push_to_DB(insert_cmd, cmd => {
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@hashedpass", hashedpass);
                cmd.Parameters.AddWithValue("@salt", salt);
                cmd.Parameters.AddWithValue("@phone", mobile);
            });

            NewUserFilesInitialisation.init();

            return true;
        }

        private static bool UserExists(string userName)
        {
            Database db = new(AppPaths.userDbpath);

            string command = "SELECT * FROM Users u\r\nWHERE u.username = @username;\r\n";

            List<Dictionary<string, object>> rows = db.Fetch_from_DB(command, cmd =>
            {
                cmd.Parameters.AddWithValue("username", userName);
            });
            return rows.Count > 0;
        }
    }
}
