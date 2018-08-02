using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Battleships.DAL
{
    public class Game
    {
        public Game()
        {
            Id = Guid.NewGuid();
            Status = GameStatuses.Started;
            StartDate = DateTime.Now;
            GameInfo = new GameInfo()
            {
                Game = this
            };
        }

        public Game(Player player)
        {
            Id = Guid.NewGuid();
            Status = GameStatuses.Started;
            StartDate = DateTime.Now;
            GameInfo = new GameInfo()
            {
                Game = this,
                FirstPlayer = player
            };
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public GameStatuses Status { get; set; }

        //If false = first, true = second
        //I do this to make my objects cheaper
        public bool Winner { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public GameInfo GameInfo { get; set; }


        public void AddPlayer(Player player)
        {
            if (GameInfo.SecondPlayer != null) { throw new Exception("There already 2 players"); }
            GameInfo.SecondPlayer = player;
        }


    }

    public enum GameStatuses
    {
        Waiting,
        Started,
        Finished
    }
}
