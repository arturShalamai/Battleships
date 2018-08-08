using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships.Api
{
    public static class API
    {

        public static string AccessToken { get; set; }

        public static class Scores
        {
            public static string SendScore(string baseUri) => $"{baseUri}/api/Scores/SendScore";
        }
    }
}
