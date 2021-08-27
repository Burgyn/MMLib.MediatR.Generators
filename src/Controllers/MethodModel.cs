using System.Collections.Generic;
using System.Linq;

namespace MMLib.MediatR.Generators.Controllers
{
    internal partial record MethodModel
    {
        public string Name { get; init; }

        public string Template { get; init; }

        public string HttpMethod { get; init; }

        public string ResponseType { get; init; }

        public string Comment { get; init; }

        public IEnumerable<string> Attributes { get; set; }

        public List<ParameterModel> Parameters { get; init; } = new();

        public string RequestType { get; init; }

        public List<string> RequestProperties { get; init; } = new();
    }
}