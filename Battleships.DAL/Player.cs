﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Battleships.DAL
{
    public class Player
    {
        public Player()
        {
            Id = Guid.NewGuid();
            GamesInfo = new List<GamePlayer>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public DateTime DoB { get; set; }

        [Required]
        public string NickName { get; set; }

        public double Score { get; set; }

        public List<GamePlayer> GamesInfo { get; set; }
    }
}
