using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Battleships.Api
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

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(conf =>
            {
                conf.Authority = "https://localhost:44362";
                conf.Audience = "https://localhost:44362/resources";
            });

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = "Cookies";
            //    options.DefaultChallengeScheme = "oidc";
            //})
            //.AddCookie("Cookies")
            //.AddOpenIdConnect("oidc", "OpenID Connect", options =>
            //{
            //    options.SignInScheme = "idsrv.external";
            //    options.SignOutScheme = "idsrv";

            //    options.Authority = "https://localhost:44362/";
            //    options.ClientId = "26fad7e9-995c-4b6b-9d16-cc2ca93d19cf";

            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        //NameClaimType = "name",
            //        //RoleClaimType = "role"
            //    };
            //});
            //.AddJwtBearer(conf =>
            //{
            //    conf.Audience = "http://localhost:44362";
            //    conf.Audience = "a7f36d6c-3502-4cac-8236-a8fd98c97e5a";
            //});

            //services.AddAuthentication("Bearer")
            //    .AddIdentityServerAuthentication(options =>
            //    {
            //        // SET THIS TO true IN PRODUCTION!
            //        options.RequireHttpsMetadata = false;

            //        options.Authority = "https://localhost:44362";
            //        options.ApiName = $"Games.Battleships";
            //    });

            services.AddCors(conf =>
            {
                conf.AddPolicy("AllowAll", opts => opts.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseCors("AllowAll");

            app.UseMvc();
        }
    }
}
