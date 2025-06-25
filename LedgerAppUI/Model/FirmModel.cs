using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedgerAppUI.Services;
using System.IO;
namespace LedgerAppUI.Model
{
    class FirmModel
    {
        public bool Add_account(CurrentSession s , string name , string group , long? opening_bal , bool? credit , string date , string? address , string? city , string? state , string? mob1 , string? mob2 , string? PAN)
        {

            Database db = new(AppPaths.CurrentUserAccountDbPath);

            string fetch_cmd = "SELECT a.name FROM Accounts a WHERE a.name = @name;";

            if (db.Fetch_from_DB(fetch_cmd, cmd =>
                                              {
                                                cmd.Parameters.AddWithValue("@name", name);
                                              }
               ).Count != 0)
            {
                return false;
            }

            string insert_cmd = "a";
            db.push_to_DB(insert_cmd);
            if(opening_bal  != null)
            {
                //make entry in EntryLog;
            }
            return true;

        }
    }
}
