﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Battleships.Api.Hubs;
using Battleships.Api.Services;
using Battleships.BLL;
using Battleships.BLL.Repos;
using Battleships.BLL.Services;
using Battleships.DAL;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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

            services.AddSignalR();

            RegisterDependencies(services);



            services.AddDbContext<BattleshipsContext>(conf =>
                            conf.UseSqlServer(Configuration.GetConnectionString("MSSQLConnectionString"),
                                              opts => opts.MigrationsAssembly("Battleships.Migrations")));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddIdentityServerJWT()
            .AddCustomJWT(Configuration);

            services.AddAuthorization(config =>
            {
                config.DefaultPolicy = new AuthorizationPolicyBuilder()
                                                            .RequireAuthenticatedUser()
                                                            .AddAuthenticationSchemes("IdentityServer", "SelfSigned")
                                                            .Build();

                //config.AddPolicy("PlatformOnly", opts => opts.AddAuthenticationSchemes("IdentityServer").Build());
            });

            services.AddCors(conf =>
            {
                conf.AddPolicy("AllowAll", opts => opts.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            });

            var disco = DiscoveryClient.GetAsync("https://localhost:44362").Result;            
            var tokenClient = new TokenClient(disco.TokenEndpoint, clientId: Configuration["Platform:ClientId"], clientSecret: Configuration["Platform:ClientSecret"]);
            // client-$bfe28474-8297-4638-bcc9-14a7f3a9a7d3
            // secret-cb60ba17-c78f-436e-b9bd-20cda037befe
            var tokenResponse = tokenClient.RequestClientCredentialsAsync("Platform.ProfileService").Result;

            API.AccessToken = tokenResponse.AccessToken;

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

            app.UseSignalR(conf => conf.MapHub<GameHub>("/hubs/game"));

            app.UseMvc();
        }

        public void RegisterDependencies(IServiceCollection services)
        {
            services.AddAutoMapper();

            services.AddScoped(typeof(IRepository<>), typeof(GenericRepo<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
            services.AddScoped<IScoreService, ScoreService>();
        }
    }
}
