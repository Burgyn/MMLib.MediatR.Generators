using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MMLib.MediatR.Generators.Demo.Applications.Base
{
    public class DeleteCommandHandler<TEntity> : IRequestHandler<DeleteCommandBase<TEntity>, Unit>
        where TEntity : class
    {
        public Task<Unit> Handle(DeleteCommandBase<TEntity> request, CancellationToken cancellationToken)
            => Unit.Task;
    }
}
