using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MMLib.MediatR.Generators.Controllers;

namespace MMLib.MediatR.Generators.Demo.Applications.Commands
{
    public class UpdatePerson
    {
        [HttpPut("{id:int}", Controller = "People")]
        [AdditionalParameters("id")]
        public record Command([property: JsonIgnore] int Id, string FirstName, string LastName) : IRequest<Unit>;

        public class Handler : IRequestHandler<Command, Unit>
        {
            public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
                => Unit.Task;
        }
    }
}
