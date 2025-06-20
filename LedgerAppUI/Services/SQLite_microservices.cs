using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Xml;

// Handle exceptions of SQLite
namespace LedgerAppUI.Services
{
    public class Database
    {
        readonly string file_ptr;

        public Database(string file_name)
        {
            file_ptr = file_name;
        }
        public void createNewDB(string CreateNewDBCommand)
        { 
            // command form : string createTableSql = @"
                //CREATE TABLE IF NOT EXISTS Users(
                //    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                //    Username TEXT NOT NULL UNIQUE,
                //    Salt TEXT NOT NULL,
                //    HashedPassword TEXT NOT NULL
                //);"

            using var connection = new SQLiteConnection($"Data Source={file_ptr};") ;
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = CreateNewDBCommand;
            command.ExecuteNonQuery();
        }

        public void push_to_DB(string Insert_command , Action<SQLiteCommand>? parameters = null)
        // command form "INSERT INTO Users (Username, Salt, HashedPassword) VALUES (@username, @salt, @hash);"
        {
            using var connection = new SQLiteConnection($"Data Source={file_ptr};");
            connection.Open();

            using var command = new SQLiteCommand(Insert_command, connection);
            parameters?.Invoke(command);
            command.ExecuteNonQuery();

        }

        public List<Dictionary<string, object>> Fetch_from_DB(string Fetch_command , Action<SQLiteCommand>? parameters = null)
        {
            var result = new List<Dictionary<string, object>>();

            if (!System.IO.File.Exists(file_ptr)) return result; // if file does not exist

            using var connection = new SQLiteConnection($"Data Source={file_ptr};");
            connection.Open();

            using var command = new SQLiteCommand(Fetch_command , connection);
            parameters?.Invoke(command); // used to call a delegate which may point to void

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var row = new Dictionary<string, object>();

                for(int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.GetValue(i);
                }
                result.Add(row);
            }

            return result;

        }

        public void Update_DB(string Update_command, Action<SQLiteCommand>? parameters = null)
        {
            using var connection = new SQLiteConnection($"Data Source={file_ptr};");
            connection.Open();

            using var command = new SQLiteCommand(Update_command, connection);
            parameters?.Invoke(command);
            command.ExecuteNonQuery();
        }

        public void Delete_from_DB(string Delete_command, Action<SQLiteCommand>? parameters = null)
        {
            using var connection = new SQLiteConnection($"Data Source={file_ptr};");
            connection.Open();

            using var command = new SQLiteCommand(Delete_command, connection);
            parameters?.Invoke(command);
            command.ExecuteNonQuery();
        }
    }

}
