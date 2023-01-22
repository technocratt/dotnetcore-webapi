using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using webapi.Utils;

namespace webapi.Controllers;

public abstract class WebApiBaseController : ControllerBase
{
    protected bool IsValidUpdate(object existingData)
    {
        return HttpContext.Request.Headers.ContainsKey(HeaderNames.IfMatch)
            && HttpContext.Request.Headers[HeaderNames.IfMatch].FirstOrDefault() != existingData.ToETag()
            ? false
            : true;
    }
}