﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships.Web.Hubs
{
    public interface IGameHub
    {
        Task StartGame(string gameId);
    }
}
