using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LedgerAppUI.Services
{
    public static class AppPaths
    {
        public static readonly string BahiFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Bahi-Khata");
        public static readonly string UserDataPath = Path.Combine(BahiFolderPath, "UserData");
        public static readonly string userDbpath = Path.Combine(UserDataPath, "User.db");
        public static readonly string CurrentUserPath = Path.Combine(BahiFolderPath, CurrentSession.SessionName);
        public static readonly string CurrentUserAccountDbPath = Path.Combine(CurrentUserPath , "accounts");
        public static readonly string CurrentUserGroupDbPath = Path.Combine(CurrentUserPath, CurrentSession.SessionName + "groups");

    }
}
