using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MMLib.MediatR.Generators.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MMLib.MediatR.Generators.Controllers
{
    internal partial record ControllerModel
    {
        internal static ControllersModelBuilder Builder(GeneratorExecutionContext context)
            => new(context.Compilation, context);

        private static ControllerModel Build(string name,
            IEnumerable<MethodCandidate> methods,
            Compilation compilation,
            Templates templates)
        {
            var ret = new ControllerModel()
            {
                Name = name,
                Namespace = $"{compilation.AssemblyName}.Controllers",
                Methods = methods.Select(m => MethodModel.Build(m, templates, name))
            };

            return ret;
        }

        internal class ControllersModelBuilder
        {
            private readonly Compilation _compilation;
            private readonly GeneratorExecutionContext _context;
            private readonly Dictionary<string, List<MethodCandidate>> _controllers = new(StringComparer.OrdinalIgnoreCase);

            public ControllersModelBuilder(
                Compilation compilation,
                GeneratorExecutionContext context)
            {
                _compilation = compilation;
                _context = context;
            }

            public void AddCandidate(TypeDeclarationSyntax candidate)
            {
                var attribute = candidate.GetAttribute(HttpMethods.Attributes);
                var semanticModel = _compilation.GetSemanticModel(candidate.SyntaxTree);
                var controllerName = attribute
                    .GetStringArgument(nameof(HttpMethodAttribute.Controller))
                    .CheckControllerName();

                if (!_controllers.ContainsKey(controllerName))
                {
                    _controllers.Add(controllerName, new List<MethodCandidate>());
                }
                var requestType = semanticModel.GetDeclaredSymbol(candidate);
                _controllers[controllerName].Add(new(attribute, semanticModel, candidate, requestType.ToDisplayString()));
            }

            public IEnumerable<ControllerModel> Build(Templates templates)
                => _controllers.Select(p => ControllerModel.Build(p.Key, p.Value, _compilation, templates));
        }
    }
}
