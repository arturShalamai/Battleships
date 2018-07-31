using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Battleships.Api.Services
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync(string userEmail);
    }
}
