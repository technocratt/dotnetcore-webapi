using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using System.Net;
using webapi.Models;

namespace webapi.Utils;

[AttributeUsage(AttributeTargets.Method)]
public class ETagFilter : ActionFilterAttribute, IAsyncActionFilter
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext executingContext, ActionExecutionDelegate next)
    {
        var request = executingContext.HttpContext.Request;
        var executedContext = await next();
        var response = executedContext.HttpContext.Response;

        if (request.Method == HttpMethod.Get.Method && response.StatusCode == (int)HttpStatusCode.OK)
        {
            var result = (BaseModel)(executedContext.Result as ObjectResult).Value;
            response.Headers.Add(HeaderNames.ETag, new[] { result.ToETag() });
        }
    }
}