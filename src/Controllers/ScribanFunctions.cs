using Scriban.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMLib.MediatR.Generators.Controllers
{
    internal class ScribanFunctions : ScriptObject
    {
        public static string MethodBody(string controllerName, MethodModel method, Templates templates)
            => SourceCodeGenerator
                .RenderBody(method, templates.GetMethodBodyTemplate(controllerName, method.HttpMethod, method.Name));

        public static string GetParameter(IEnumerable<ParameterModel> parameters, string requestType)
            => parameters is not null
                ? GetRequerstParameter(parameters, requestType)
                : $"new {requestType}()";

        private static string GetRequerstParameter(IEnumerable<ParameterModel> parameters, string requestType)
            => parameters?.FirstOrDefault(p 
                => p.Type.Equals(requestType, StringComparison.CurrentCultureIgnoreCase))?.Name;

        public static string PostInitiate(
            string request,
            IEnumerable<ParameterModel> parameters,
            List<string> requestProperties)
        {
            var additionalParameters = parameters
                ?.Where(p => p.CanPostInitiateCommand)?.ToList();

            if (additionalParameters?.Any() != true)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            foreach (var parameter in additionalParameters)
            {
                var property = requestProperties.First(p =>
                    p.Equals(parameter.Name, StringComparison.CurrentCultureIgnoreCase));
                sb.AppendLine($"{GetRequerstParameter(parameters, request)}.{property} = {parameter.Name};");
            }

            return sb.ToString();
        }
    }
}