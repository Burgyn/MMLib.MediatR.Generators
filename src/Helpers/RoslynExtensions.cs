using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MMLib.MediatR.Generators.Helpers
{
    internal static class RoslynExtensions
    {
        public static CompilationUnitSyntax GetCompilationUnit(this SyntaxNode syntaxNode)
            => syntaxNode.Ancestors().OfType<CompilationUnitSyntax>().FirstOrDefault();

        public static bool HaveAttribute(this TypeDeclarationSyntax typeDeclaration, string attributeName)
            => typeDeclaration?.AttributeLists.Count > 0
                && typeDeclaration
                    .AttributeLists
                       .SelectMany(SelectWithAttributes(attributeName))
                       .Any();

        public static bool HaveAnyOfAttributes(this TypeDeclarationSyntax typeDeclaration, ISet<string> attributesName)
            => typeDeclaration?.AttributeLists.Count > 0
                && typeDeclaration
                    .AttributeLists
                       .SelectMany(SelectWithAttributes(attributesName))
                       .Any();

        public static IEnumerable<AttributeSyntax> GetAttributes(
            this TypeDeclarationSyntax typeDeclaration,
            string attributeName)
            => typeDeclaration
                .AttributeLists
                    .SelectMany(SelectWithAttributes(attributeName));

        public static IEnumerable<AttributeSyntax> GetAttributes(
            this TypeDeclarationSyntax typeDeclaration,
            ISet<string> attributesName)
            => typeDeclaration
                .AttributeLists
                    .SelectMany(SelectWithAttributes(attributesName));

        public static AttributeSyntax GetAttribute(this TypeDeclarationSyntax typeDeclaration, string attributeName)
            => typeDeclaration.GetAttributes(attributeName).FirstOrDefault();

        public static AttributeSyntax GetAttribute(this TypeDeclarationSyntax typeDeclaration, ISet<string> attributesName)
            => typeDeclaration.GetAttributes(attributesName).FirstOrDefault();

        private static Func<AttributeListSyntax, IEnumerable<AttributeSyntax>> SelectWithAttributes(string attributeName)
            => l => l?.Attributes.Where(a => (a.Name as IdentifierNameSyntax)?.Identifier.Text == attributeName);

        private static Func<AttributeListSyntax, IEnumerable<AttributeSyntax>> SelectWithAttributes(ISet<string> attributes)
            => l => l?.Attributes.Where(a => attributes.Contains((a.Name as IdentifierNameSyntax)?.Identifier.Text));

        public static string GetClassName(this TypeDeclarationSyntax typeDeclaration)
            => typeDeclaration.Identifier.Text;

        public static string GetClassModifier(this TypeDeclarationSyntax typeDeclaration)
            => typeDeclaration.Modifiers.ToFullString().Trim();

        public static string GetNamespace(this CompilationUnitSyntax root)
            => root.ChildNodes()
                .OfType<NamespaceDeclarationSyntax>()
                .First().Name.ToString();

        public static bool ContainsArguments(this AttributeSyntax attribute, string argumentName)
            => attribute
               .ArgumentList
               .Arguments
               .Any(p => p.NameEquals.Name.Identifier.ValueText == argumentName);

        public static string GetStringArgument(
            this AttributeSyntax attribute,
            string argumentName,
            SemanticModel semanticModel)
        {
            var value = attribute.GetArgument<LiteralExpressionSyntax>(argumentName);

            return value.Token.ValueText;
        }

        public static HashSet<string> GetArrayArguments(
            this AttributeSyntax attribute,
            string argumentName,
            SemanticModel semanticModel,
            Func<string, string> processItem)
        {
            HashSet<string> ret = new();
            SeparatedSyntaxList<ExpressionSyntax>? expressions =
                attribute.GetArgument<ArrayCreationExpressionSyntax>(argumentName)?.Initializer.Expressions;

            if (expressions != null)
            {
                foreach (var expression in expressions)
                {
                    Optional<object> value = semanticModel.GetConstantValue(expression);
                    if (value.HasValue)
                    {
                        ret.Add(processItem(value.Value.ToString()));
                    }
                }
            }

            return ret;
        }

        public static string GetConstantAttribute(
            this AttributeSyntax attribute,
            string argumentName,
            SemanticModel semanticModel)
        {
            var expression = attribute.GetArgument<ExpressionSyntax>(argumentName);
            Optional<object> value = semanticModel.GetConstantValue(expression);

            return value.HasValue ? value.Value.ToString() : null;
        }

        private static T GetArgument<T>(this AttributeSyntax attribute, string argumentName)
            where T : ExpressionSyntax
            => attribute
                .ArgumentList
                .Arguments
                .FirstOrDefault(p => p.NameEquals?.Name.Identifier.ValueText == argumentName)?.Expression as T;

        public static IEnumerable<ITypeSymbol> GetBaseTypesAndThis(this ITypeSymbol type)
        {
            ITypeSymbol current = type;
            while (current != null)
            {
                yield return current;
                current = current.BaseType;
            }
        }
    }
}
