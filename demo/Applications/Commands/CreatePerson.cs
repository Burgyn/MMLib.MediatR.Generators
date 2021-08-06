using MediatR;
using MMLib.MediatR.Generators.Controllers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MMLib.MediatR.Generators.Demo.Applications.Commands
{
    public class CreatePerson
    {
        [HttpPost(Controller = "People")]
        public record Command(string FirstName, string LastName): IRequest<int>;

        public class Handler : IRequestHandler<Command, int>
        {
            public Task<int> Handle(Command request, CancellationToken cancellationToken)
                => Task.FromResult(new Random().Next());
        }
    }
}
