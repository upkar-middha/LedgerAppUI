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
        public bool Add_account(string name, string group, long? opening_bal, bool? credit, string date,
                        string? address, string? city, string? state,
                        string? mob1, string? mob2, string? PAN)
        {
            Database db = new(AppPaths.CurrentUserAccountDbPath);

            // Step 1: Check for existing account
            string fetch_cmd = "SELECT a.name FROM Accounts a WHERE a.name = @name;";

            if (db.Fetch_from_DB(fetch_cmd, cmd =>
            {
                cmd.Parameters.AddWithValue("@name", name);
            }).Count != 0)
            {
                return false; // account with same name exists
            }

            // Step 2: Insert new account (some fields nullable)
            string insert_cmd = @"
                                INSERT INTO Accounts (name, grp, reg_date, op_bal, address, city, state, mobile1, mobile2, pan)
                                VALUES (@name, @grp, @date, @op_bal, @address, @city, @state, @mob1, @mob2, @pan);";

            db.push_to_DB(insert_cmd, cmd =>
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@grp", group);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@op_bal", opening_bal ?? 0);           // default to 0 if null
                cmd.Parameters.AddWithValue("@address", (object?)address ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@city", (object?)city ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@state", (object?)state ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@mob1", (object?)mob1 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@mob2", (object?)mob2 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@pan", (object?)PAN ?? DBNull.Value);
            });

            // Step 3: If opening balance exists, log it
            if (opening_bal != null)
            {
                EntryLog.MakeEntry(name, date, opening_bal.Value, credit ?? true);
            }

            return true;
        }


        public List<string>? SearchAccounts()
        {
            return { "a" };
        
    }
}
