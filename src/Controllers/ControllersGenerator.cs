using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using MMLib.MediatR.Generators.Helpers;
using System;
using System.IO;
using System.Text;

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
                foreach (ClassDeclarationSyntax candidate in actorSyntaxReciver.Candidates)
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

        static Templates LoadTemplates(GeneratorExecutionContext context)
        {
            Templates templates = new();

            foreach (AdditionalText file in context.AdditionalFiles)
            {
                if (Path.GetExtension(file.Path).Equals(".txt", StringComparison.OrdinalIgnoreCase))
                {
                    var options = context.AnalyzerConfigOptions.GetOptions(file);
                    if (options.TryGetValue("build_metadata.additionalfiles.TemplateType", out string type) &&
                        Enum.TryParse(type, ignoreCase: true, out TemplateType templateType))
                    {
                        string controllerName = string.Empty;
                        if (options.TryGetValue("build_metadata.additionalfiles.ControllerName", out string name))
                        {
                            controllerName = name;
                        }

                        templates.AddTemplate(templateType, controllerName, file.GetText(context.CancellationToken).ToString());
                    }
                }
            }

            return templates;
        }
    }
}
