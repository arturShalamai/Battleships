using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Battleships.DAL
{
    public class Player
    {
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

        public List<Game> Games { get; set; }
    }
}
