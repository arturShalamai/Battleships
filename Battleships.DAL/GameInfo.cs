using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Battleships.DAL
{
    public class GameInfo
    {
        public GameInfo()
        {
            FirstUserField = new string(' ', 42);
            SecondUserField = FirstUserField;
        }

        public Guid GameId { get; set; }
        public bool FirstUserReady { get; set; }
        public string FirstUserField { get; set; }

        public string SecondUserField { get; set; }
        public bool SecondUserReady { get; set; }
        //To make my objects cheaper
        //If true = first User, false = second
        //Especially for in memory store
        public bool Turn { get; set; }

        public Game Game { get; set; }
    }







}
