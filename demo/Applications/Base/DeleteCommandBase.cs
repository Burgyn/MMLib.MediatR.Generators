using MediatR;

namespace MMLib.MediatR.Generators.Demo.Applications.Base
{
    public abstract class DeleteCommandBase<TEntity> : IRequest<Unit>
        where TEntity : class
    {
        public int Id { get; set; }
    }
}
