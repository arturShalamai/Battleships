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
            GameInfo = new GameInfo();
            PlayersInfo = new List<GamePlayer>();
        }

        public Game(Player player)
        {
            Id = Guid.NewGuid();
            Status = GameStatuses.Waiting;
            StartDate = DateTime.Now;
            GameInfo = new GameInfo();
            PlayersInfo = new List<GamePlayer>
            {
                new GamePlayer()
                {
                    Player = player,
                    Game = this
                }
            };
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public GameStatuses Status { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        //If true = first, false = second
        //I do this to make my objects cheaper
        public bool Winner { get; set; }

        public GameInfo GameInfo { get; set; }

        public List<GamePlayer> PlayersInfo { get; set; }


        public void AddPlayer(Player player)
        {
            if (PlayersInfo.Count == 2) { throw new Exception("There already 2 players"); }
            PlayersInfo.Add(new GamePlayer()
            {
                Player = player,
                Game = this
            });
        }


    }

    public enum GameStatuses
    {
        Waiting,
        Started,
        Finished
    }
}
