using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using webapi.Services;

namespace webapi.Security;

[AttributeUsage(AttributeTargets.Method)]
public class ApiReadAuthorization : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext filterContext)
    {
        var user = filterContext?.HttpContext?.User?.Identity;

        if (user == null || !user.IsAuthenticated || string.IsNullOrEmpty(user.Name))
        {
            filterContext.Result = new ForbidResult();
        }

        var userService = filterContext.HttpContext.RequestServices.GetService<IUserService>();
        var userExists = userService.UserExists(user.Name);

        if (!userExists)
        {
            filterContext.Result = new ForbidResult();
        }
    }
}