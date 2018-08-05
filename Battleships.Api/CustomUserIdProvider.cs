using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Battleships.Api.Hubs
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            //var check = connection.User.HasClaim(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            //var id = connection.User?.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var claimsIdentity = (ClaimsIdentity)connection.User.Identity;
            return claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value; ;
        }
    }
}
