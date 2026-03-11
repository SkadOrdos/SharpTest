using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace WebAppi.Auth
{
    public class AuthorizationMiddlewareHandler : IAuthorizationMiddlewareResultHandler
    {
        IUserProvider _userProvider;
        public AuthorizationMiddlewareHandler(IUserProvider userProvider)
        {
            _userProvider = userProvider;
        }


        private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();

        public async Task HandleAsync(
            RequestDelegate next,
            HttpContext context,
            AuthorizationPolicy policy,
            PolicyAuthorizationResult authorizeResult)
        {
            context.Response.Headers[HeaderNames.AccessControlAllowHeaders] = "*";
            context.Response.Headers[HeaderNames.AccessControlAllowOrigin] = "*";

            // If the authorization was forbidden and the resource had a specific requirement,
            // provide a custom 404 response.
            if (authorizeResult.Forbidden)
            {
                // Return a 404 to make it appear as if the resource doesn't exist.
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            try
            {
                if (context.Request.Method == HttpMethod.Post.ToString())
                {
                    string authHeader = context.Request.Headers.Authorization.ToString();
                    if (String.IsNullOrEmpty(authHeader))
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    }
                    else
                    {
                        // Populate user: adjust claims as needed
                        var creds = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Replace("Basic ", ""))).Split(':');
                        if (creds.Length >= 2 && _userProvider.VerifyUser(creds[0], creds[1]))
                        {
                            var claims = new[] { new Claim(ClaimTypes.Name, creds[0], ClaimValueTypes.String) };
                            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));
                            context.Request.HttpContext.User = principal;
                            //return;
                        }
                        else
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        }
                    }
                }
                else if (context.Request.Method == HttpMethod.Options.ToString())
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            // Fall back to the default implementation.
            await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }

}
