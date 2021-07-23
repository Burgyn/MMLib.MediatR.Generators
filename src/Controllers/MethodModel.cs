using System.Collections.Generic;

namespace MMLib.MediatR.Generators.Controllers
{
    internal partial class MethodModel
    {
        public string Name { get; private set; }

        public string Template { get; private set; }

        public string HttpMethod { get; private set; }

        public string MethodSignatureTemplate { get; private set; }

        public string MethodBodyTemplate { get; private set; }

        public string ResponseType { get; private set; }

        public IEnumerable<ParameterModel> Parameters { get; set; }

        public IEnumerable<string> Attributes { get; set; }
    }
}
