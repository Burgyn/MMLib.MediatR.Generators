using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MMLib.MediatR.Generators.Helpers;
using System.Collections.Generic;

namespace MMLib.MediatR.Generators.Controllers
{
    internal sealed class ControllerReceiver : ISyntaxReceiver
    {
        private readonly List<ClassDeclarationSyntax> _candidates = new();

        public IEnumerable<ClassDeclarationSyntax> Candidates => _candidates;

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classSyntax && classSyntax.HaveAnyOfAttributes(HttpMethods.Attributes))
            {
                _candidates.Add(classSyntax);
            }
        }
    }
}
