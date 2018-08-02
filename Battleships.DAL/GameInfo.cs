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
            FirstPlayerField = new string(' ', 42);
            SecondPlayerField = FirstPlayerField;
        }

        public GameInfo(Player firstPlayer)
        {
            //FirstPlayer = firstPlayer;

            FirstPlayerField = new string(' ', 42);
            SecondPlayerField = FirstPlayerField;

        }

        public Guid GameId { get; set; }
        public Game Game { get; set; }

        public Guid FirstPlayerId { get; set; }
        public Player FirstPlayer { get; set; }
        public string FirstPlayerField { get; set; }

        public Guid SecondPlayerId { get; set; }
        public Player SecondPlayer { get; set; }
        public string SecondPlayerField { get; set; }
    }
}
