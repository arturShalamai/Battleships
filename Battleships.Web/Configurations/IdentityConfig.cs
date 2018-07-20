using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships.Web.Configurations
{
    public static class IdentityConfig
    {
        public static string SignInScheme = "Cookies";

        public static string Authority = "https://localhost:44362/";

        public static string ClientId = "Games.Battleships";

        public static string ClientSecret = "secret";
    }
}
