using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Battleships.DAL
{
    public class PlayerCredentials
    {
        public PlayerCredentials()
        {
            PlayerId = Guid.NewGuid();
        }

        [Key]
        public Guid PlayerId { get; set; }


        public Player Player { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [MaxLength(15)]
        [MinLength(7)]
        public string Password { get; set; }
    }
}
