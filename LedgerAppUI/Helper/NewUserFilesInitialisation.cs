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
            Directory.CreateDirectory(AppPaths.CurrentUserPath); // create a new directory to keep data of new user
            Database db = new(AppPaths.CurrentUserAccountDbPath);
            string cmd = "CREATE TABLE IF NOT EXISTS Accounts (\r\n  name VARCHAR(40) PRIMARY KEY,\r\n  grp VARCHAR(40) NOT NULL,\r\n  reg_date DATE NOT NULL,\r\n  op_bal INT DEFAULT 0,\r\n  address VARCHAR(100),\r\n  city VARCHAR(20),\r\n  state VARCHAR(20),\r\n  mobile1 CHAR(10) not NULL,\r\n  mobile2 CHAR(10),\r\n  pan CHAR(10),\r\n  );";
            db.createNewDB(cmd);

            db = new(AppPaths.CurrentUserGroupDbPath);
            cmd = "CREATE TABLE IF NOT EXISTS Ledger_grps (\r\n  name VARCHAR(40) PRIMARY KEY\r\n  );";
            db.createNewDB(cmd);
        }
    }
}
