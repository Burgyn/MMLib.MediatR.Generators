using System.Collections.Generic;

namespace MMLib.MediatR.Generators.Controllers
{
    internal partial class ControllerModel
    {
        public string Namespace { get; private set; }

        public string Name { get; private set; }

        public IEnumerable<MethodModel> Methods { get; private set; }
    }
}
