using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MMLib.MediatR.Generators.Controllers
{
    internal record MethodCandidate(
        AttributeSyntax HttpMethodAttribute,
        SemanticModel SemanticModel,
        TypeDeclarationSyntax TypeDeclaration,
        string RequestType);
}
