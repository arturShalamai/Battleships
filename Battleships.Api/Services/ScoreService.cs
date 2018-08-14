using Battleships.Api;
using Battleships.Api.Models;
using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.BLL.Services
{
    class ScoreService : IScoreService
    {
        private readonly HttpClient _client;

        public ScoreService()
        {
            _client = new HttpClient();
        }

        public async Task SendScores(Guid userId, double scores)
        {
            _client.SetBearerToken(API.AccessToken);
            var resp = await _client.PostAsJsonAsync(API.Scores.SendScore("https://localhost:44350"), new UserScorePostModel(userId, scores));
        }
    }
}
