using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MMLib.MediatR.Generators.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MMLib.MediatR.Generators.Controllers
{
    internal partial class ControllerModel
    {
        internal static ControllersModelBuilder Builder(GeneratorExecutionContext context)
            => new(context.Compilation, context);

        private static ControllerModel Build(string name,
            IEnumerable<MethodCandidate> methods,
            Compilation compilation,
            GeneratorExecutionContext context)
        {
            var ret = new ControllerModel()
            {
                Name = name,
                Namespace = $"{compilation.AssemblyName}.Controllers"
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

            public void AddCandidate(ClassDeclarationSyntax candidate)
            {
                AttributeSyntax attribute = candidate.GetAttribute(HttpMethods.Attributes);
                SemanticModel semanticModel = _compilation.GetSemanticModel(candidate.SyntaxTree);
                string controllerName = attribute
                    .GetStringArgument(nameof(HttpGetAttribute.Controller), semanticModel)
                    .CheckControllerName();

                if (!_controllers.ContainsKey(controllerName))
                {
                    _controllers.Add(controllerName, new List<MethodCandidate>());
                }
                _controllers[controllerName].Add(new(attribute, semanticModel));
            }

            public IEnumerable<ControllerModel> Build()
                => _controllers.Select(p => ControllerModel.Build(p.Key, p.Value, _compilation, _context));
        }

        public record MethodCandidate(AttributeSyntax HttpMethodAttribute, SemanticModel SemanticModel);
    }
}
