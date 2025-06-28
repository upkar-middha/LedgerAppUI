using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LedgerAppUI.Services;

namespace LedgerAppUI.Helper
{
    class NewUserFilesInitialisation
    {
        public static void init()
        {
            // Create directory
            Directory.CreateDirectory(AppPaths.CurrentUserPath);

            // Create Accounts table
            Database db = new(AppPaths.CurrentUserAccountDbPath);
            string cmd = @"
                            CREATE TABLE IF NOT EXISTS Accounts (
                              name VARCHAR(40) PRIMARY KEY,
                              grp VARCHAR(40) NOT NULL,
                              reg_date TEXT NOT NULL,           
                              address VARCHAR(100),
                              city VARCHAR(20),
                              state VARCHAR(20),
                              mobile1 CHAR(10) NOT NULL,
                              mobile2 CHAR(10),
                              pan CHAR(10)
                            );";
            db.createNewDB(cmd);

            // Create Ledger_grps table
            db = new(AppPaths.CurrentUserGroupDbPath);
            cmd = @"
                    CREATE TABLE IF NOT EXISTS Ledger_grps (
                      name VARCHAR(40) PRIMARY KEY
                    );";
            db.createNewDB(cmd);

            // Create Bahi table
            db = new(AppPaths.CurrentUserBahiPath);
            cmd = @"
                        CREATE TABLE IF NOT EXISTS Bahi (
                          name TEXT,
                          date TEXT,       
                          balance INTEGER,
                          grp TEXT
                        );";
            db.createNewDB(cmd);

        }
    }
}
