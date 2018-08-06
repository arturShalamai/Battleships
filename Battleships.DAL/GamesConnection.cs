using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Battleships.DAL
{
    public class GamesConnection
    {
        [Key]
        public int Id { get; set; }

        public string ConnectionId { get; set; }
        public Guid GameId { get; set; }
        public Guid UserId { get; set; }

        public Player Player { get; set; }
        public Game Game { get; set; }

    }
}
