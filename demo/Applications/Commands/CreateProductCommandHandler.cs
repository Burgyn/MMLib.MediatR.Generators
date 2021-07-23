using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MMLib.MediatR.Generators.Demo.Applications.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        public Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
            => Task.FromResult(new Random().Next());
    }
}
