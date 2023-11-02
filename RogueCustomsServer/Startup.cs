using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RogueCustomsServer.Middlewares;
using Roguelike.Controllers;
using Roguelike.Services;
using System.Collections.Generic;

namespace RogueCustomsServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddMemoryCache();

            services.AddTransient<ExceptionMiddleware>();
            services.AddTransient<DungeonMiddleware>();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(config =>
                {
                    config.ConfigObject.AdditionalItems["syntaxHighlight"] = new Dictionary<string, object>
                    {
                        ["activated"] = false
                    };
                });
            }

            app.UseCors();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<DungeonMiddleware>();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
