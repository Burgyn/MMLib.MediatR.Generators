using AutoBogus;
using Kros.Extensions;
using MediatR;
using MMLib.MediatR.Generators.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MMLib.MediatR.Generators.Demo.Applications.Queries
{
    public class GetAllPeople
    {
        [HttpGet("all", Controller = "People", From = From.Ignore)]
        public record Query() : IRequest<IEnumerable<Response>>;

        [HttpGet(Controller = "People", Name = "GetPeople", From = From.Query)]
        public record QueryPager(int Skip, int Take) : IRequest<IEnumerable<Response>>;

        public record Handler :
            IRequestHandler<Query, IEnumerable<Response>>,
            IRequestHandler<QueryPager, IEnumerable<Response>>
        {
            public Task<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
                => AutoFaker.Generate<Response>(10).AsTask();

            public Task<IEnumerable<Response>> Handle(QueryPager request, CancellationToken cancellationToken)
                => AutoFaker.Generate<Response>(100).Skip(request.Skip).Take(request.Take).AsTask();
        }

        public record Response(int Id, string LastName);
    }
}
