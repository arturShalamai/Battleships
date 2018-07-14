using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Battleships.DAL
{
    public class GameInfo
    {
        public Guid GameId { get; set; }

        public string FirstUserField { get; set; }

        public string SecondUserField { get; set; }
        //To make my objects cheaper
        //If true = first User, false = second
        //Especially for in memory store
        public bool Turn { get; set; }

        public Game Game { get; set; }
    }
}
