using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace webapi.Security;

public class ApiAuthenticationHandler : AuthenticationHandler<ApiAuthenticationOptions>
{
    public ApiAuthenticationHandler(IOptionsMonitor<ApiAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
            {
                new Claim(ClaimTypes.Name, "app-user"),
                new Claim(ClaimTypes.Role, "admin")
            };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        Thread.CurrentPrincipal = principal;
        return Task.Run(() =>
        {
            return AuthenticateResult.Success(ticket);
        });
    }
}