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
        public static readonly string userDbpath = Path.Combine(BahiFolderPath, "User.db");

    }
}
