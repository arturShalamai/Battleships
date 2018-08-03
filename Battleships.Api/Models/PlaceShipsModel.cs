using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships.Api.Models
{
    public class PlaceShipsModel
    {
        public Guid GameId { get; set; }
        public string Field { get; set; }
    }
}
