using System;
using System.Collections.Generic;

namespace MMLib.MediatR.Generators.Controllers
{
    internal static class HttpMethods
    {
        public const string Get = "Get";
        public const string Post = "Post";
        public const string Put = "Put";
        public const string Delete = "Delete";
        public const string Patch = "Patch";

        public static readonly ISet<string> Attributes = new HashSet<string>(StringComparer.OrdinalIgnoreCase){
            HttpMethod(Get), HttpMethod(Post), HttpMethod(Put), HttpMethod(Delete), HttpMethod(Patch) };

        private static string HttpMethod(string type) => $"Http{type}";
    }
}
