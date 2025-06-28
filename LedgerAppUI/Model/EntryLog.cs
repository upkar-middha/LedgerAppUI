using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using LedgerAppUI.Services;
namespace LedgerAppUI.Model
{
    class EntryLog
    {
        public static void MakeEntry(string name, string date, long balance, bool credit)
        {
            Database db = new(AppPaths.CurrentUserBahiPath);

            string insertQuery = @"INSERT INTO Bahi (name, date, balance, grp) 
                           VALUES (@name, @date, @balance, @grp);";

            if (!credit) balance *= -1;

            string group = fetch_group_name(name); // fetch from Accounts table

            db.push_to_DB(insertQuery, cmd =>
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@balance", balance);
                cmd.Parameters.AddWithValue("@grp", group);
            });
        }


        public static List<Dictionary<string, object>> Find_by_name(string name)
        {
            Database db = new(AppPaths.CurrentUserBahiPath);

            string searchQuery = "SELECT * FROM Bahi WHERE name = @name";

            return db.Fetch_from_DB(searchQuery, cmd =>
            {
                cmd.Parameters.AddWithValue("@name", name);
            });
        }

        public static List<Dictionary<string, object>> Find_more_than_date(string date)
        {
            Database db = new(AppPaths.CurrentUserBahiPath);

            string query = "SELECT * FROM Bahi WHERE date >= @date";

            return db.Fetch_from_DB(query, cmd =>
            {
                cmd.Parameters.AddWithValue("@date", date);
            });
        }

        public static List<Dictionary<string, object>> Find_less_than_date(string date)
        {
            Database db = new(AppPaths.CurrentUserBahiPath);

            string query = "SELECT * FROM Bahi WHERE date <= @date";

            return db.Fetch_from_DB(query, cmd =>
            {
                cmd.Parameters.AddWithValue("@date", date);
            });
        }

        public static List<Dictionary<string, object>> Find_by_date(string date)
        {
            Database db = new(AppPaths.CurrentUserBahiPath);

            string query = "SELECT * FROM Bahi WHERE date = @date";

            return db.Fetch_from_DB(query, cmd =>
            {
                cmd.Parameters.AddWithValue("@date", date);
            });
        }

        public static List<Dictionary<string, object>> Find_more_than(string balance)
        {
            Database db = new(AppPaths.CurrentUserBahiPath);

            string query = "SELECT * FROM Bahi WHERE balance >= @balance";

            return db.Fetch_from_DB(query, cmd =>
            {
                cmd.Parameters.AddWithValue("@balance", balance);
            });
        }

        public static List<Dictionary<string, object>> Find_less_than(string balance)
        {
            Database db = new(AppPaths.CurrentUserBahiPath);

            string query = "SELECT * FROM Bahi WHERE balance <= @balance";

            return db.Fetch_from_DB(query, cmd =>
            {
                cmd.Parameters.AddWithValue("@balance", balance);
            });
        }

        public static List<Dictionary<string, object>> Find_by_group(string groupName)
        {
            Database db = new(AppPaths.CurrentUserBahiPath);

            string query = @"
                            SELECT Bahi.*
                            FROM Bahi
                            INNER JOIN Accounts ON Bahi.name = Accounts.name
                            WHERE Accounts.grp = @group";

            return db.Fetch_from_DB(query, cmd =>
            {
                cmd.Parameters.AddWithValue("@group", groupName);
            });
        }


        private static string fetch_group_name(string name)
        {
            Database db = new(AppPaths.CurrentUserAccountDbPath);

            string fetch_cmd = "SELECT grp FROM Accounts WHERE name = @name";

            List<Dictionary<string, object>> rows = db.Fetch_from_DB(fetch_cmd, cmd =>
            {
                cmd.Parameters.AddWithValue("@name", name);
            });

            if (rows.Count == 0) return "";

            return rows[0]["grp"].ToString();
        }

    }
}
