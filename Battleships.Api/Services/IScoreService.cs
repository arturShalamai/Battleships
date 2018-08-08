using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.BLL.Services
{
    public interface IScoreService
    {
        Task SendScores(Guid userId, double score);
    }
}
