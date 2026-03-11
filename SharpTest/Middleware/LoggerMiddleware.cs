namespace WebSharp
{
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;
        public LoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Console.WriteLine($"[{DateTime.UtcNow}] {context.Connection.RemoteIpAddress.ToString()} >>> {context.Request.Method} {context.Request.Path}");
            await _next(context);
            return;
        }
    }
}
