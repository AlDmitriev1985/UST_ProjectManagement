using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UST_ProjectManagement
{
    public static class GlobalSettingsClass
    {
        public static string MainPath = @"Z:\BIM02\";

        public static string ConnectionString = ConfigurationManager.ConnectionStrings["TestConnection"].ConnectionString;
    }
}
