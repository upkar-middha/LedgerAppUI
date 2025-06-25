using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedgerAppUI.Services;
using LedgerAppUI.Helper;

namespace LedgerAppUI.Model
{
    class AccountModificationSettings
    {
        public static void changePassword(string new_password)
        {
            string username = CurrentSession.SessionName;

            string salt = SaltGeneration.Salt(10);
            char pepper = (char)('a' + Random.Shared.Next(26));
            string hashed_pass = HashStringUsingSHA.Hash(salt + new_password + pepper);

            Database db = new(AppPaths.userDbpath);

            // check if new_password is different from the old password
            if (Authentication.login(username, new_password))
            {
                throw new ArgumentException("New password cannot be the same as the old password.");
            }

            string cmd = "UPDATE Users\r\nSET hashedpass = @hashed_pass, salt = @salt\r\nWHERE username = @username;\r\n";

            db.Update_DB(cmd, parameter =>
            {
                parameter.Parameters.AddWithValue("@hashed_pass", hashed_pass);
                parameter.Parameters.AddWithValue("@salt", salt);
                parameter.Parameters.AddWithValue("@username", username);
            }
            );

        }

        public void change_Mobile(string new_mobile)
        {
            string username = CurrentSession.SessionName;

            Database db = new(AppPaths.userDbpath);

            string cmd = "UPDATE Users\r\nSET phone = @phone\r\nWHERE username = @username;\r\n";

            db.Update_DB(cmd, parameter =>
            {
                parameter.Parameters.AddWithValue("@phone", new_mobile);
                parameter.Parameters.AddWithValue("@username", username);
            });
        }
    }
}
