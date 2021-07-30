using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Scriban;
using Scriban.Runtime;
using System.Text;

namespace MMLib.MediatR.Generators.Controllers
{
    internal class SourceCodeGenerator
    {
        public static SourceText Generate(ControllerModel controller, Templates templates)
        {
            string output = RenderBody(new
            {
                Usings = templates.GetControllerTemplate(TemplateType.ControllerUsings, controller.Name),
                Attributes = RenderControllerAttributes(controller, templates),
                Body = RenderControllerBody(controller, templates),
                Controller = controller
            }, templates.GetControllerTemplate(TemplateType.Controller, controller.Name));

            output = Format(output);

            return SourceText.From(output, Encoding.UTF8);
        }

        private static string Format(string output)
        {
            var tree = CSharpSyntaxTree.ParseText(output);
            var root = (CSharpSyntaxNode)tree.GetRoot();
            output = root.NormalizeWhitespace(elasticTrivia: true).ToFullString();

            return output;
        }

        private static string RenderControllerAttributes(ControllerModel controller, Templates templates)
            => RenderBody(controller, templates.GetControllerTemplate(TemplateType.ControllerAttributes, controller.Name));

        private static string RenderControllerBody(ControllerModel controller, Templates templates)
            => RenderBody(new
                {
                    Controller = controller,
                    controller.Methods,
                    templates
                }, templates.GetControllerTemplate(TemplateType.ControllerBody, controller.Name));

        public static string RenderBody(object body, string templateSource)
        {
            var template = Template.Parse(templateSource);
            TemplateContext context = CreateContext(body);

            return template.Render(context);
        }

        private static TemplateContext CreateContext(object body)
        {
            var context = new TemplateContext();

            var scriptObject = new ScriptObject();
            scriptObject.Import(body);
            context.PushGlobal(scriptObject);

            var functions = new ScribanFunctions();
            context.PushGlobal(functions);

            return context;
        }
    }
}
