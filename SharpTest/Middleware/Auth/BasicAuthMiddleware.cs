using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Microsoft.Net.Http.Headers;
using System.Text;
using System.Net;

namespace WebSharp.Auth
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUserProvider _userProvider;

        public BasicAuthMiddleware(IUserProvider userProvider, RequestDelegate next)
        {
            _userProvider = userProvider;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.Headers[HeaderNames.AccessControlAllowHeaders] = "*";
            context.Response.Headers[HeaderNames.AccessControlAllowOrigin] = "*";

            if (context.Request.Method == HttpMethod.Options.ToString())
            {
                return;
            }

            // Проверяем заголовок Authorization
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                var authHeader = context.Request.Headers["Authorization"].ToString();
                if (authHeader.StartsWith("Basic "))
                {
                    var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
                    var credentialBytes = Convert.FromBase64String(encodedCredentials);
                    var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');

                    if (credentials.Length >= 2 && _userProvider.VerifyUser(credentials[0], credentials[1]))
                    {
                        var claims = new[] { new Claim(ClaimTypes.Name, credentials[0], ClaimValueTypes.String) };
                        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));
                        context.Request.HttpContext.User = principal;

                        // Успешная авторизация, вызываем следующий Middleware
                        await _next(context);
                        return;
                    }
                }
            }

            // Если авторизация не удалась, возвращаем 401
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.Headers["WWW-Authenticate"] = "Basic";
            await context.Response.WriteAsync("Unauthorized");
        }
    }
}
