using Api.Models;

namespace Api.Extensions
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
        public static readonly HashSet<string> BlackListTokens = new();

        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (context.Request.Headers.ContainsKey("Authorization"))
                {
                    var token = context.Request.Headers["Authorization"].FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        if (BlackListTokens.Contains(token))
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            await context.Response.WriteAsync(string.Empty);
                            return;
                        }
                    }
                    ApiContext.Current.Token = token ?? string.Empty;
                }
                else if (context.Request.Headers.ContainsKey("authorization"))
                {
                    var token = context.Request.Headers["authorization"].FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        if (BlackListTokens.Contains(token))
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            await context.Response.WriteAsync(string.Empty);
                            return;
                        }
                    }
                    ApiContext.Current.Token = token ?? string.Empty;
                }
            }
            catch { }
            await _next(context);
        }
    }
}
