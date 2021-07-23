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
        public void ShouldGetTemplateByControllerName(string controller, TemplateType type, string template)
        {
            var templates = new Templates();
            templates.AddTemplate(type, controller, template);

            string actual = templates.GetTemplate(type, controller);

            actual.Should().Be(template);
        }

        [Theory]
        [InlineData(TemplateType.Controller)]
        [InlineData(TemplateType.ControllerAttributes)]
        [InlineData(TemplateType.ControllerBody)]
        [InlineData(TemplateType.ControllerUsings)]
        public Task ShouldGetDefaultTemplateIfDoesnotExist(TemplateType type)
        {
            var templates = new Templates();

            string actual = templates.GetTemplate(type, "NonExisting");

            return Verifier.Verify(actual)
                .UseParameters(type);
        }
    }
}
