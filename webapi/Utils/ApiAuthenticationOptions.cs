using Microsoft.AspNetCore.Authentication;

namespace webapi.Security;

public class ApiAuthenticationOptions : AuthenticationSchemeOptions
{
    public string Realm = "security";
}