using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Scriban;
using System.Text;

namespace MMLib.MediatR.Generators.Controllers
{
    internal class SourceCodeGenerator
    {
        public static SourceText Generate(ControllerModel controller, Templates templates)
        {
            var template = Template.Parse(templates.GetTemplate(TemplateType.Controller, controller.Name));

            string output = template.Render(new
            {
                Usings = templates.GetTemplate(TemplateType.ControllerUsings, controller.Name),
                Attributes = RenderControllerAttributes(controller, templates),
                Body = RenderControllerBody(controller, templates),
                Controller = controller
            }, member => member.Name);

            output = Format(output);

            return SourceText.From(output, Encoding.UTF8);
        }

        private static string Format(string output)
        {
            var tree = CSharpSyntaxTree.ParseText(output);
            var root = (CSharpSyntaxNode)tree.GetRoot();
            output = root.NormalizeWhitespace().ToFullString();

            return output;
        }

        private static string RenderControllerAttributes(ControllerModel controller, Templates templates)
        {
            var template = Template.Parse(templates.GetTemplate(TemplateType.ControllerAttributes, controller.Name));

            return template.Render(controller, member => member.Name);
        }

        private static string RenderControllerBody(ControllerModel controller, Templates templates)
        {
            var template = Template.Parse(templates.GetTemplate(TemplateType.ControllerBody, controller.Name));

            return template.Render(new
            {
                Controller = controller,
                controller.Methods
            }, member => member.Name);
        }
    }
}
