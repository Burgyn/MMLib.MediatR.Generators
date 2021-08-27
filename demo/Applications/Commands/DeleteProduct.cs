using MMLib.MediatR.Generators.Controllers;
using MMLib.MediatR.Generators.Demo.Applications.Base;
using MMLib.MediatR.Generators.Demo.Domains;

namespace MMLib.MediatR.Generators.Demo.Applications.Commands
{
    /// <summary>
    /// Delete product.
    /// </summary>
    [HttpDelete("{id:int}", Controller = "Products")]
    public class DeleteProduct : DeleteCommandBase<Product>
    {
    }
}
