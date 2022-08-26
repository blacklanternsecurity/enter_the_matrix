using Enter_The_Matrix.Services;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Enter_The_Matrix.Services
{

    public class ApiAuthorizationService
    {
        private readonly RequestDelegate _next;
        private const string APIKEY = "X-Api-Key";
        private readonly KeyService _keyService;

        public ApiAuthorizationService(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, KeyService keyService)
        {
            if (!context.Request.Headers.TryGetValue(APIKEY, out var keyIn))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Missing API Key");
                return;
            }

            keyIn = context.Request.Headers[APIKEY];

            if (!await keyService.Validate(keyIn, context.Request.Method, context.Request.Path.ToString().Split("/")[2]))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }
            await _next(context);
        }
    }
}