using FluentAssertions;
using MMLib.MediatR.Generators.Controllers;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace MMLib.MediatR.Generators.Tests.Controllers
{
    [UsesVerify]
    public class TemplatesShould
    {
        [Theory]
        [InlineData("People", TemplateType.Controller, "template for whole controller.")]
        [InlineData("", TemplateType.Controller, "default template for all controllers.")]
        [InlineData("Users", TemplateType.ControllerAttributes, "Template for users controller attributes.")]
        [InlineData("", TemplateType.ControllerAttributes, "Default template for all controllers attributes.")]
        [InlineData("Users", TemplateType.ControllerBody, "Template for people controller body.")]
        [InlineData("", TemplateType.ControllerBody, "Default template for all controllers body.")]
        [InlineData("Users", TemplateType.ControllerUsings, "Template for people controller usings.")]
        [InlineData("", TemplateType.ControllerUsings, "Default template for all controllers usings.")]
        [InlineData("Users", TemplateType.MethodAttributes, "[Attribute1],[Attribute2]")]
        [InlineData("", TemplateType.MethodAttributes, "[Attribute3],[Attribute4]")]
        public void ShouldGetTemplateByControllerName(string controller, TemplateType type, string template)
        {
            var templates = new Templates();
            templates.AddTemplate(type, controller, template);

            var actual = templates.GetControllerTemplate(type, controller);

            actual.Should().Be(template);
        }

        [Theory]
        [InlineData(TemplateType.Controller)]
        [InlineData(TemplateType.ControllerAttributes)]
        [InlineData(TemplateType.ControllerBody)]
        [InlineData(TemplateType.ControllerUsings)]
        [InlineData(TemplateType.MethodAttributes)]
        public Task ShouldGetDefaultTemplateIfDoesNotExist(TemplateType type)
        {
            var templates = new Templates();

            string actual = templates.GetControllerTemplate(type, "NonExisting");

            return Verifier.Verify(actual)
                .UseParameters(type);
        }

        [Theory]
        [InlineData("People", "Get", "GetAll", "Get all template.")]
        [InlineData("", "Get", "", "Default Get method body.")]
        [InlineData("People", "Post", "GetAll", "Post template.")]
        [InlineData("", "Post", "", "Default Post method body.")]
        public void ShouldGetMethodBodyTemplate(string controller, string httpType, string methodName, string template)
        {
            var templates = new Templates();
            templates.AddMethodBodyTemplate(controller, httpType, methodName, template);

            var actual = templates.GetMethodBodyTemplate(controller, httpType, methodName);

            actual.Should().Be(template);
        }

        [Theory]
        [InlineData("Get")]
        [InlineData("Post")]
        [InlineData("Put")]
        [InlineData("Delete")]
        [InlineData("Patch")]
        public Task ShouldGetDefaultMethodBodyTemplateIfDoesNotExist(string httpType)
        {
            var templates = new Templates();

            var actual = templates.GetMethodBodyTemplate("controller", httpType , "NonExisting");

            return Verifier.Verify(actual)
                .UseParameters(httpType);
        }
    }
}
