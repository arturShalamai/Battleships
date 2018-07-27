using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Battleships.Configs;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
              .AddCookie()
              .AddOpenIdConnect(options =>
              {
                    // === FOR DEMO ONLY
                    options.RequireHttpsMetadata = false;
                    // SET THIS TO true IN PRODUCTION!

                    options.GetClaimsFromUserInfoEndpoint = true;
                  options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                  options.Authority = ExternalWebAppOptions.IdentityServerHost;
                  options.ClientId = ExternalWebAppOptions.ClientId;
                  options.ClientSecret = ExternalWebAppOptions.ClientSecret;

                  options.ResponseType = "code id_token";
                  options.SaveTokens = true;

                  options.Scope.Add("offline_access");
                  options.Scope.Add("profile");
              });


            //services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            //    .AddIdentityServerAuthentication(options =>
            //    {
            //        // base-address of your identityserver
            //        options.Authority = "https://localhost:44362/";

            //        // name of the API resource
            //        options.ApiName = "ApiName";
            //        options.ApiSecret = "ApiSecret";
            //    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
