using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Battleships.BLL.Models
{
    public class PlayerLoginModel
    {
        public PlayerLoginModel()
        {

        }

        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
