using MediatR;
using MMLib.MediatR.Generators.Controllers;

namespace MMLib.MediatR.Generators.Demo.Applications.Commands
{
    [HttpPost(Controller = "Products")]
    public class CreateProductCommand: IRequest<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
