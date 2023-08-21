using Api.Models;

namespace Api.Extensions
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
        public static readonly Dictionary<string, DateTime> BlackListTokens = new();

        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;
            _ = Task.Run(() => RemoveTokenTask());
        }

        private static void RemoveTokenTask()
        {
            while (true)
            {
                var needToRemove = new List<string>();
                foreach (var token in BlackListTokens)
                {
                    if (token.Value <= DateTime.Now) needToRemove.Add(token.Key);
                }
                foreach (var key in needToRemove)
                {
                    BlackListTokens.Remove(key);
                }
                Thread.Sleep(60 * 1000);
            }
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
                        if (BlackListTokens.ContainsKey(token))
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
                        if (BlackListTokens.ContainsKey(token))
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
