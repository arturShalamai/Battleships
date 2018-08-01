using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Battleships.BLL
{
    public class PlayerRegisterModel
    {
        public PlayerRegisterModel()
        {

        }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string NickName { get; set; }

        [Required]
        [MinLength(7)]
        [MaxLength(15)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MinLength(7)]
        [MaxLength(15)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
