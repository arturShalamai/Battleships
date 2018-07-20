using Battleships.Web.Configurations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships.Web.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddIdentityAuthorization(this AuthenticationBuilder builder)
        {
            builder.AddOpenIdConnect("oidc", options =>
             {
                 options.SignInScheme = "Cookies";

                 options.Authority = IdentityConfig.Authority;
                 options.RequireHttpsMetadata = false;

                 options.ClientId = IdentityConfig.ClientId;
                 options.ClientSecret = IdentityConfig.ClientSecret;
                 options.ResponseType = "code id_token";

                 options.SaveTokens = true;
                 options.GetClaimsFromUserInfoEndpoint = true;

                 //options.Scope.Add("api1");
                 options.Scope.Add("profile");
                 options.Scope.Add("offline_access");
             });
        }
    }
}
