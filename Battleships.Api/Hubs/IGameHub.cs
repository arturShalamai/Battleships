using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships.Api.Hubs
{
    public interface IGameHub
    {
        Task MakeTurn(int pos);

        Task StartGame(string gameId);

        Task EnemyReady();

        //Task Shipes();
    }
}
