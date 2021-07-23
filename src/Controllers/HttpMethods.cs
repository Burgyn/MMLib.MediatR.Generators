using System;
using System.Collections.Generic;

namespace MMLib.MediatR.Generators.Controllers
{
    internal static class HttpMethods
    {
        public static ISet<string> Attributes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            { "HttpGet", "HttpPost", "HttpPut", "HttpDelete" };
    }
}
