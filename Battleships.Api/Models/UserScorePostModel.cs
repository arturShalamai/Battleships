using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships.Api.Models
{
    public class UserScorePostModel
    {

        public Guid UserId { get; set; }
        public double Scores { get; set; }

        public UserScorePostModel(Guid userId, double scores)
        {
            UserId = userId;
            Scores = scores;
        }
    }
}
