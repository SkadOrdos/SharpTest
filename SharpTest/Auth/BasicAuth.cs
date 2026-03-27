using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;


namespace WebSharp.Auth
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserProvider _userProvider;
        public BasicAuthenticationHandler(IUserProvider userProvider, IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
            : base(options, logger, encoder)
        {
            this._userProvider = userProvider;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));

            try
            {
                var header = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

                if (!"Basic".Equals(header.Scheme, StringComparison.OrdinalIgnoreCase))
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Scheme"));

                byte[] credentialBytes = Convert.FromBase64String(header.Parameter);
                string[] credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
                if (credentials.Length != 2)
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Credentials Format"));

                if (credentials.Length >= 2 && _userProvider.VerifyUser(credentials[0], credentials[1]))
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, credentials[0], ClaimValueTypes.String) };
                    var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, Scheme.Name));

                    // Успешная авторизация
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return Task.FromResult(AuthenticateResult.Success(ticket));
                }
            }
            catch (Exception ex)
            {

            }

            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Headers["WWW-Authenticate"] = "Basic realm=\"api\"";
            return base.HandleChallengeAsync(properties);
        }
    }
}
