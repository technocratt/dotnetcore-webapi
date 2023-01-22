using Newtonsoft.Json;

namespace webapi.Utils;

public static class Extensions
{
    public static string ToETag(this object obj)
    {
        return JsonConvert.SerializeObject(obj).GetHashCode().ToString();
    }
}