using MMLib.MediatR.Generators.Helpers;
using System;
using System.Collections.Generic;

namespace MMLib.MediatR.Generators.Controllers
{
    internal class Templates
    {
        private readonly Dictionary<string, string> _templates = new();

        public void AddTemplate(TemplateType type, string controllerName, string template)
        {
            _templates[GetName(type, controllerName)] = template;
        }

        public void AddMethodBodyTemplate(string controllerName, string httpType, string methodName, string template)
        {
            _templates[GetMethodBodyTemplateName(controllerName, httpType, methodName)] = template;
        }

        public string GetControllerTemplate(TemplateType type, string controllerName)
            => _templates.ContainsKey(GetName(type, controllerName))
                ? _templates[GetName(type, controllerName)]
                : _templates.ContainsKey(GetName(type, string.Empty))
                    ? _templates[GetName(type, string.Empty)]
                    : type switch
                    {
                        TemplateType.Controller => EmbeddedResource.GetContent("Controllers.Templates.Controller.txt"),
                        TemplateType.ControllerAttributes=> EmbeddedResource.GetContent("Controllers.Templates.ControllerAttributes.txt"),
                        TemplateType.ControllerUsings => EmbeddedResource.GetContent("Controllers.Templates.Usings.txt"),
                        TemplateType.ControllerBody => EmbeddedResource.GetContent("Controllers.Templates.Method.txt"),
                        TemplateType.MethodAttributes => null,
                        TemplateType.MethodBody => null,
                        _ => throw new ArgumentOutOfRangeException(nameof(type), $"Unexpected template type: {type}.")
                    };

        public string GetMethodBodyTemplate(string controllerName, string httpType, string methodName)
            => _templates.ContainsKey(GetMethodBodyTemplateName(controllerName, httpType, methodName))
                ? _templates[GetMethodBodyTemplateName(controllerName, httpType, methodName)]
                : _templates.ContainsKey(GetMethodBodyTemplateName(string.Empty, httpType, string.Empty))
                    ? _templates[GetMethodBodyTemplateName(string.Empty, httpType, string.Empty)]
                    : EmbeddedResource.GetContent($"Controllers.Templates.Http{httpType}MethodBody.txt");

        private static string GetName(TemplateType type, string controllerName)
            => $"{type}-{controllerName}";

        private static string GetMethodBodyTemplateName(string controllerName, string httpType, string methodName)
            => $"{controllerName}-{httpType}-{methodName}";
    }
}
