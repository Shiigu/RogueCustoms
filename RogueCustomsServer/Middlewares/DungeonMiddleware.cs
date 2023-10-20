using Microsoft.AspNetCore.Http;
using RogueCustomsGameEngine.Management;
using System.Threading.Tasks;
using System;
using Roguelike.Services;

namespace RogueCustomsServer.Middlewares
{
    public class DungeonMiddleware : IMiddleware
    {
        private readonly DungeonService dungeonService;

        public DungeonMiddleware(DungeonService dungeonService)
        {
            this.dungeonService = dungeonService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Extract the "dungeonId" from the route data.
            if (context.Request.RouteValues.TryGetValue("dungeonId", out var dungeonIdValue) &&
                int.TryParse(dungeonIdValue.ToString(), out int dungeonId))
            {
                dungeonService.UpdateAccessTimeAndCleanupUnusedDungeons(dungeonId);
            }

            await next(context);
        }
    }
}
