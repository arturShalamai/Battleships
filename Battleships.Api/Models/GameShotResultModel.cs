using Battleships.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships.Api.Models
{
    public class GameShotResultModel
    {
        public Guid GameId { get; set; }
        public int Position { get; set; }
        public string Result { get; set; }
    }
}
