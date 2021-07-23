using MMLib.MediatR.Generators.Controllers;
using MMLib.MediatR.Generators.Demo.Applications.Base;
using MMLib.MediatR.Generators.Demo.Domains;

namespace MMLib.MediatR.Generators.Demo.Applications.Commands
{
    [HttpDelete("{id:int}", Controller = "People")]
    [FromRoute]
    public class DeletePerson : DeleteCommandBase<Person>
    {
    }
}
