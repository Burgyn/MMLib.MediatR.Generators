using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MMLib.MediatR.Generators.Controllers
{
    public static class GeneratorExecutionContextExtensions
    {
        private static readonly DiagnosticDescriptor _missingArgument = new(
            id: "MMLG001",
            title: "Missing attribute argument",
            messageFormat: "Argument '{0}' of '{1}Attribute' is required",
            category: "MMLib.MediatR.Generators",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static void ReportMissingArgument(
            this GeneratorExecutionContext context,
            AttributeSyntax attribute,
            string argumentName)
            => context.ReportDiagnostic(
                Diagnostic.Create(
                    _missingArgument,
                    attribute.GetLocation(),
                    argumentName,
                    (attribute.Name as IdentifierNameSyntax)?.Identifier.Text));

    }
}