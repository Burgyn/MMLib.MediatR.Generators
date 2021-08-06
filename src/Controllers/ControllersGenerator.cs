using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using MMLib.MediatR.Generators.Helpers;
using System;
using System.IO;
using System.Text;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MMLib.MediatR.Generators.Controllers
{
    [Generator]
    public class ControllersGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new ControllerReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            context.AddSource("SourceTypes.cs",
                SourceText.From(EmbeddedResource.GetContent("Controllers.SourceTypes.cs"), Encoding.UTF8));

            if (context.SyntaxReceiver is ControllerReceiver actorSyntaxReciver)
            {
                var builder = ControllerModel.Builder(context);
                foreach (TypeDeclarationSyntax candidate in actorSyntaxReciver.Candidates)
                {
                    builder.AddCandidate(candidate);
                }

                var templates = LoadTemplates(context);
                foreach (var controller in builder.Build())
                {
                    context.AddSource($"{controller.Name}", SourceCodeGenerator.Generate(controller, templates));
                }
            }
        }

        private static Templates LoadTemplates(GeneratorExecutionContext context)
        {
            Templates templates = new();

            foreach (var file in context.AdditionalFiles)
            {
                if (Path.GetExtension(file.Path).Equals(".txt", StringComparison.OrdinalIgnoreCase))
                {
                    var options = context.AnalyzerConfigOptions.GetOptions(file);
                    if (options.TryGetValue("build_metadata.additionalfiles.TemplateType", out var type) &&
                        Enum.TryParse(type, ignoreCase: true, out TemplateType templateType))
                    {
                        var controllerName = TryGetValue(options, "ControllerName");
                        var template  = file.GetText(context.CancellationToken).ToString();

                        if (templateType != TemplateType.MethodBody)
                        {
                            templates.AddTemplate(templateType, controllerName, template);
                        }
                        else
                        {
                            var methodType = TryGetValue(options, "MethodType");
                            var methodName = TryGetValue(options, "MethodName");
                            templates.AddMethodBodyTemplate(controllerName, methodType, methodName, template);
                        }
                    }
                }
            }

            return templates;
        }

        private static string TryGetValue(AnalyzerConfigOptions options, string type)
            => options.TryGetValue($"build_metadata.additionalfiles.{type}", out var value) ? value : string.Empty;
    }
}
