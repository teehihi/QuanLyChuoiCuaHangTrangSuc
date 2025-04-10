using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer
{
    public static class ConnectionHelper
    {
        public static string CurrentConnectionString { get; set; }
        public static string CurrentUserName { get; set; }
        public static bool IsManager { get; set; } 
    }
}
