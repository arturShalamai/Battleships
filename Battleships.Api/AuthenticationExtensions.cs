using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Api
{
    public static class AuthenticationExtensions
    {
        public static AuthenticationBuilder AddIdentityServerJWT(this AuthenticationBuilder builder)
        {
            return builder.AddJwtBearer("IdentityServer", conf =>
            {
                conf.Authority = "https://localhost:44362";
                conf.Audience = "https://localhost:44362/resources";
            });
        }

        public static AuthenticationBuilder AddCustomJWT(this AuthenticationBuilder builder, IConfiguration configuration)
        {
            return builder.AddJwtBearer("SelfSigned", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "https://localhost:44310",
                    ValidAudience = "https://localhost:44310",
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["SecurityKey"]))
                };
            });
        }

    }
}

