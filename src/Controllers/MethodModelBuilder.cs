using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MMLib.MediatR.Generators.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MMLib.MediatR.Generators.Controllers
{
    internal partial record MethodModel
    {
        public static MethodModel Build(MethodCandidate candidate)
        {
            string httpType = GetMethodType(candidate);
            string name = GetMethodName(candidate);
            string template = GetTemplate(candidate);

            return new MethodModel()
            {
                HttpMethod = httpType,
                Name = name,
                Template = template,
                Parameters = GetParameters(candidate, httpType),
                RequestType = candidate.RequestType,
                ResponseType = GetResponseType(candidate, httpType)
            };
        }

        private static string GetTemplate(MethodCandidate candidate)
            => candidate.HttpMethodAttribute.GetFirstArgumentWithoutName();

        private static string GetMethodName(MethodCandidate candidate)
        {
            string name = candidate.HttpMethodAttribute
                .GetStringArgument(nameof(HttpGetAttribute.Name));

            if (!string.IsNullOrWhiteSpace(name))
            {
                return name;
            }

            name = candidate.TypeDeclaration.GetTypeName();
            if (candidate.TypeDeclaration.Parent is ClassDeclarationSyntax classDeclaration)
            {
                name = $"{classDeclaration.GetTypeName()}{name}";
            }

            return name;
        }

        private static string GetMethodType(MethodCandidate candidate)
            => candidate.HttpMethodAttribute.Name.ToString().Replace(Types.Http, string.Empty);

        private static IEnumerable<ParameterModel> GetParameters(MethodCandidate candidate, string httpMethod)
        {
            List<ParameterModel> parameters = new();
            var methodFromSource
                = candidate.HttpMethodAttribute.GetArgument<MemberAccessExpressionSyntax>(nameof(HttpMethodAttribute.From));
            if (!Enum.TryParse(methodFromSource?.Name.Identifier.ValueText, out From methodFrom))
            {
                methodFrom = httpMethod switch
                {
                    HttpMethods.Get => From.Route,
                    HttpMethods.Delete => From.Route,
                    HttpMethods.Post => From.Body,
                    HttpMethods.Put => From.Body,
                    _ => From.Ignore
                };
            }

            if (methodFrom != From.Ignore)
            {
                parameters.Add(new ParameterModel(
                    GetParameterName(httpMethod),
                    candidate.RequestType,
                    AttributeFrom(methodFrom)));
            }

            return parameters;
        }

        private static string GetParameterName(string httpMethod) => httpMethod == HttpMethods.Get ? "query" : "command";

        private static string AttributeFrom(From methodFrom) => $"[From{methodFrom}]";

        private static string GetResponseType(MethodCandidate candidate, string httpMethod)
        {
            string type = candidate
                .HttpMethodAttribute
                .GetTypeArgument(nameof(HttpMethodAttribute.ResponseType), candidate.SemanticModel)?.ToDisplayString();

            if (string.IsNullOrWhiteSpace(type))
            {
                if (httpMethod == HttpMethods.Get)
                {
                    return GetResponseTypeForGetMethod(candidate);
                }
                else
                {
                    return Types.ActionResult;
                }
            }

            return type;
        }

        private static string GetResponseTypeForGetMethod(MethodCandidate candidate)
        {
            string type = string.Empty;
            var request = candidate.TypeDeclaration.BaseList.Types.AsEnumerable()
                .FirstOrDefault(p =>
                    p.Type is GenericNameSyntax r
                    && r.Identifier.ValueText == Types.Request
                    && r.TypeArgumentList.Arguments.Count == 1)?.Type as GenericNameSyntax;

            if (request is not null)
            {
                var responseType = request.TypeArgumentList.Arguments.First();
                if (responseType.Kind() == SyntaxKind.PredefinedType)
                {
                    type = (responseType as PredefinedTypeSyntax).Keyword.ValueText;
                }
                else
                {
                    var info = candidate.SemanticModel.GetTypeInfo(request.TypeArgumentList.Arguments.First());
                    type = info.Type.ToDisplayString();
                }
            }

            type = type.Replace(Types.EnumerableGenericNamespaces, string.Empty);

            if (type == Types.Unit)
            {
                type = Types.ActionResult;
            }

            return type;
        }
    }
}