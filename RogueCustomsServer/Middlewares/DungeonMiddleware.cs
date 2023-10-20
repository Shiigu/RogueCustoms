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
        private readonly RequestDelegate next;

        public DungeonMiddleware(RequestDelegate next, DungeonService dungeonService)
        {
            this.next = next;
            this.dungeonService = dungeonService;
        }

        public async Task Invoke(HttpContext context)
        {
            // Extract the "dungeonId" from the route data.
            if (context.Request.RouteValues.TryGetValue("dungeonId", out var dungeonIdValue) &&
                int.TryParse(dungeonIdValue.ToString(), out int dungeonId))
            {
                dungeonService.UpdateAccessTimeAndCleanupUnusedDungeons(dungeonId);
            }

            await next(context);
        }

        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            return Invoke(context);
        }
    }
}
