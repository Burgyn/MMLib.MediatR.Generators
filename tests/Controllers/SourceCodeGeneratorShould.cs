using Microsoft.CodeAnalysis.Text;
using MMLib.MediatR.Generators.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace MMLib.MediatR.Generators.Tests.Controllers
{
    [UsesVerify]
    public class SourceCodeGeneratorShould
    {
        [Fact]
        public Task GenerateControllerWithoutMethods()
        {
            var model = new ControllerModel()
            {
                Name = "People",
                Namespace = "MMLib.Demo.Controllers"
            };
            SourceText actual = SourceCodeGenerator.Generate(model, new Templates());

            return Verifier.Verify(actual);
        }

        [Fact]
        public Task GenerateController()
        {
            var methods = new List<MethodModel>()
            {
                new()
                {
                    HttpMethod = "Get", Name = "GetAll", ResponseType = "IEnumerable<Person>",
                    Comment = "Get all person.",
                    RequestType = "GetAllPeopleQuery",
                    Attributes = "[AllowAnonymous]"
                },
                new()
                {
                    HttpMethod = "Get", Template = "{id:int}", Name = "GetById", ResponseType = "Person",
                    Parameters = new List<ParameterModel>() {new("query", "GetPersonById", "[FromRoute]")},
                    RequestType = "GetPersonById",
                    Attributes = "[AllowAnonymous]\n\r[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetAllTagsQuery.Tag>))]"
                },
                new()
                {
                    HttpMethod = "Get", Template = "search", Name = "GetBySearchQuery",
                    ResponseType = "IEnumerable<Person>",
                    Parameters = new List<ParameterModel>() {new("query", "PersonSearchQuery", "[FromQuery]")},
                    RequestType = "PersonSearchQuery"
                },
                new()
                {
                    HttpMethod = "Post", Name = "CreatePerson", ResponseType = "ActionResult",
                    Parameters = new List<ParameterModel>() {new("command", "CreatePersonCommand")},
                    RequestType = "CreatePersonCommand"
                },
                new()
                {
                    HttpMethod = "Put", Template = "{id:int}", Name = "UpdatePerson", ResponseType = "ActionResult",
                    RequestProperties = new List<string>() {"Id", "FirstName", "LastName"},
                    Parameters = new List<ParameterModel>()
                    {
                        new("id", "int", string.Empty, true),
                        new("command", "UpdatePersonCommand")
                    },
                    RequestType = "UpdatePersonCommand"
                },
                new()
                {
                    HttpMethod = "Delete", Name = "DeletePerson", ResponseType = "ActionResult",
                    Parameters = new List<ParameterModel>() {new("command", "DeletePersonCommand", "[FromRoute]")},
                    RequestType = "DeletePersonCommand"
                }
            };

            var model = new ControllerModel()
            {
                Name = "People",
                Namespace = "MMLib.Demo.Controllers",
                Methods = methods
            };
            var actual = SourceCodeGenerator.Generate(model, new Templates());

            return Verifier.Verify(actual);
        }
    }
}