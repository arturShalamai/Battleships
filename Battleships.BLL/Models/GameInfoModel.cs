using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships.BLL.Models
{
    public class GameInfoModel
    {
        public Guid Id { get; set; }

        public string PlayerField { get; set; }
        public bool PlayerReady { get; set; }

        public string EnemyField { get; set; }
        public bool EnemyReady { get; set; }

        public string Turn { get; set; }

    }
}
