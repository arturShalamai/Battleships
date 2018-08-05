using System;
using System.Collections.Generic;
using System.Text;

namespace Battleships.DAL
{
    public class PlayerConnection
    {
        public PlayerConnection()
        {

        }

        public PlayerConnection(Player player)
        {
            Player = player;
            Connected = true;
        }

        public PlayerConnection(Player player, string connId)
        {
            Player = player;
            ConnectionId = connId;
            Connected = true;
        }

        public Guid Id { get; set; }

        public Guid PlayerId { get; set; }
        public string ConnectionId { get; set; }
        public bool Connected { get; set; }

        public Player Player { get; set; }
    }
}
