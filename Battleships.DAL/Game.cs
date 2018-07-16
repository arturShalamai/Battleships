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
            Status = GameStatuses.Started;
            StartDate = DateTime.Now;
            GameInfo = new GameInfo();
            PlayersInfo = new List<GamePlayer>
            {
                new GamePlayer()
                {
                    Player = player
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
    }

    public enum GameStatuses
    {
        Waiting,
        Started,
        Finished
    }
}
