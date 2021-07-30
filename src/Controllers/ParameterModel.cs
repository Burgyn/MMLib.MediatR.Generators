namespace MMLib.MediatR.Generators.Controllers
{
    internal record ParameterModel(string Name, string Type, string Attribute = "", bool CanPostInitiateCommand = false);
}
