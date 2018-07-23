using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Battleships.BLL;
using Battleships.BLL.Services;
using Battleships.Web.Extensions;
using Battleships.Web.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Battleships.Web
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
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddIdentityAuthorization();

            services.AddCors(opts =>
            {
                opts.AddPolicy("AllowAll", conf => conf.AllowAnyOrigin()
                                                        .AllowAnyHeader()
                                                        .AllowAnyMethod());
            });

            services.AddSignalR();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<IGameService, GameService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseCors("AllowAll");

            app.UseSignalR(x => x.MapHub<GameHub>("/hubs/game"));

            app.UseJwtBearerAuthentication(new JwtBearerOptions()
            {
                Audience = "http://localhost:5001/",
                Authority = "https://localhost:44362/",
                AutomaticAuthenticate = true
            };

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
