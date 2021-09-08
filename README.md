# MediatR controllers generator

This generator generates controllers and their methods for you based on your [MediatR](https://github.com/jbogard/MediatR) requests.

---

Did this project help you? [You can now buy me a beer 😎🍺.](https://www.buymeacoffee.com/0dQ7tNG)

[!["You can now buy me a beer 😎🍺."](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)](https://www.buymeacoffee.com/0dQ7tNG)

## Installation

```bash
  dotnet add package MMLib.MediatR.Generators
```

## How to use it

[MediatR](https://github.com/jbogard/MediatR)  is a great project that enables in-process messaging. Many ASP.NET Core projects use it to implement the CQRS pattern. You use MediatR to define yours commands and queries, but you still have to manually write a large amount of similar code to the controllers.

This generator will generate the necessary controllers and methods for you. Just decorate your request with an attribute according to the appropriate http method and define the name of the controller. In this case, it will be `[HttpPost(Controller = "People")]`.

```csharp
[HttpPost(Controller = "People")]
public record CreatePersonCommand(string FirstName, string LastName): IRequest<int>
```

The generator generates the following controller:

```csharp
[ApiController]
[Route("[controller]")]
public partial class PeopleController : ControllerBase
{
    private IMediator _mediator;

    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="mediator">Mediator.</param>
    public PeopleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("")]
    public async Task<ActionResult> CreatePersonCommand([FromBody] CreatePersonCommand command)
    {
        return await SendCreateCommand(command, nameof(CreatePersonCommand));
    }

    private async Task<CreatedResult> SendCreateCommand<TResponse>(IRequest<TResponse> command, string actionName = null)
    {
        var ret = new
        {
          id = await _mediator.Send(command)
        };
        string url = actionName != null ? Url.Link(actionName, ret) ?? string.Empty : string.Empty;
        return Created(url, ret);
    }
}
```

## Another example

You have the following commands and queries.

```csharp
/// <summary>
/// Testing comment.
/// </summary>
[HttpPost(Controller = "People")]
public record CreatePersonCommand(string FirstName, string LastName): IRequest<int>
{
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
    private void RequestMethodDefinition()
    {
        throw new NotImplementedException();
    }
}

[HttpDelete("{id:int}", Controller = "People")]
public class DeletePersonCommand : DeleteCommandBase<Person>{}

/// <summary>
/// Command for update person.
/// </summary>
[HttpPut("{id:int}", Controller = "People")]
[AdditionalParameters("id")]
public record UpdatePersonCommand([property:JsonIgnore]int Id, string FirstName, string LastName) : IRequest<Unit>;

[HttpGet("all", Controller = "People", From = From.Ignore)]
public record GetAllPeopleQuery() : IRequest<IEnumerable<Person>>;

[HttpGet(Controller = "People", Name = "GetPeople", From = From.Query)]
public record GetPeopleQuery(int Skip, int Take) : IRequest<IEnumerable<Person>>;

[HttpGet("{id}", Controller = "People", Name = "GetById")]
public record Query(int Id) : IRequest<Response>;
```

The generated controller looks like this:

```csharp
[ApiController]
[Route("[controller]")]
public partial class PeopleController : ControllerBase
{
    private IMediator _mediator;
    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="mediator">Mediator.</param>
    public PeopleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Testing comment.
    /// </summary>
    [HttpPost("")]
    [Microsoft.AspNetCore.Authorization.AllowAnonymous()]
    [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken()]
    public async Task<ActionResult> CreatePersonCommand([FromBody] CreatePersonCommand command)
    {
        return await SendCreateCommand(command, nameof(CreatePersonCommand));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeletePersonCommand([FromRoute] DeletePersonCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Command for update person.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdatePersonCommand([FromBody] UpdatePersonCommand command, Int32 id)
    {
        command.Id = id;
        await _mediator.Send(command);
        return Ok();
    }

    [HttpGet("all")]
    public async Task<IEnumerable<Person>> GetAllPeopleQuery()
    {
        return await _mediator.Send(new GetAllPeopleQuery());
    }

    [HttpGet("")]
    public async Task<IEnumerable<Person>> GetPeople([FromQuery] GetPeopleQuery query)
    {
        return await _mediator.Send(query);
    }

    [HttpGet("{id}")]
    public async Task<Response> GetById([FromRoute] Query query)
    {
        return await _mediator.Send(query);
    }

    private async Task<CreatedResult> SendCreateCommand<TResponse>(IRequest<TResponse> command, string actionName = null)
    {
        var ret = new
        {
          id = await _mediator.Send(command)
        };
        string url = actionName != null ? Url.Link(actionName, ret) ?? string.Empty : string.Empty;
        return Created(url, ret);
    }
}
```

## Customizations

### Http method type

Use the inherited attributes from the `HttpMethodAttribute` to specify to which type of http method to translate your command / query.  Supported types are `Get`, `Post`, `Put`, `Delete`.

### Controller name

Parameter `Controller` is required. Use this parameter to define the name of the controller and to group the methods to be within one controller.

### Method name

The method name is generated based on the Command / Query class name *(if it is a nested class, the prefix is the name of the main class)*. However, if you want to change this, you can use the `Name` property. Eg: `[HttpGet(Controller = "People", Name = "GetPeople")]`

### Parameter source type

The given command / query is used as a parameter of the generated methods. It is used with the `FromRoute` attribute for the `Get` and `Delete` methods and with the `FromBody` attribute for `Post` and `Put`. If you want to change this default behavior, use the `From` property. Possible values: `Ignore`, `Route`, `Query`, `Body`, `Header`, `Form`, `Services`. Eg.: `[HttpGet(Controller = "People", From = From.Query)]`.

> If you do not want the command / query type to be used as a parameter, use `From.Ignore`. Command / query will be created in the method `_mediator.Send(new GetAllPeopleQuery())`.

### Route template

You can define route template for the method: `[HttpDelete("{id:int}")]`.

### Response type

The response type is derived based on the generic used to define your command / query. For query definition `record GetPersonQuery() : IRequest<Person>;` it will be `Task<Person>`. If you use MediatR type `Unit` it will be `Task<ActionResult>`.

Use `ResponseType` property to define your own type. Eg.: `[HttpPost(Controller = "People", ResponseType = typeof(long))]`

### Additional parameter

Sometimes you need to add another parameter to the path, which you then want to set to the command. Typically, with the `Put` method, you want to have the record `Id` in the path and assign it to the command. You can use the attribute for this `[AdditionalParameters("id")]`.

The generated method looks like this:

```csharp
[HttpPut("{id:int}")]
public async Task<ActionResult> UpdatePersonCommand([FromBody] UpdatePersonCommand command, Int32 id)
{
    command.Id = id;
    await _mediator.Send(command);
    return Ok();
}
```

### New method to generated controller

Controllers are generated as a `partial class`es, so if you want to add a new method to this controller,  you can create a new `partial class` for this controller.

### Comments

If you have enabled the generation of a documentation file (`<GenerateDocumentationFile>true</GenerateDocumentationFile>`) in your csproj, your XML comments from the command / query definitions will be used as comments of the generated method.

## Templates

The generation consists of several templates, some of which can be overwritten in the end application.

### General

[Scriban](https://github.com/scriban/scriban) is used as templating engine. All templates you must define in `csproj` file as additional files. The `TemplateType` property determines what type of template it is. Use the property `ControllerName` to override the template for a specific controller.

### Controllers usings

In your project you can define your own `.txt` file, which will contain the `usings` part of the controller.

Your file might look like this:

```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
```

Your template must be added to `csproj` as an additional file with the `TemplateType` property set to `ControllerUsings`

```xml
<ItemGroup>
  <AdditionalFiles Include="Templates\Attributes.txt" MMLib_TemplateType="ControllerUsings" />
</ItemGroup>
```

This template is used for all generated controllers. To define a custom usage for a particular controller, you must set the controller name using the `MMLib_ControllerName` property.

```xml
<ItemGroup>
  <AdditionalFiles Include="Templates\Attributes.txt" MMLib_TemplateType="ControllerUsings" MMLib_ControllerName="ProductsController" />
</ItemGroup>
```

### Controller attributes

`MMLib_TemplateType` property must be set to `ControllerAttributes`.

### Http method body

You can override body of http method. Eg.: `HttpGetMethodBody.txt`

```csharp
{
	return await _mediator.Send({{ parameters | get_parameter request_type }});
}
```

```xml
<ItemGroup>
  <AdditionalFiles Include="Templates\HttpGetMethodBody.txt" MMLib_TemplateType="MethodBody" MMLib_MethodType="Get" />
</ItemGroup>
```

### Method attributes

If you want to override the attributes above all generated methods, you can also do so via a template. `MMLib_TemplateType` property must be set to `MethodAttributes`.

But if you want to define specific attributes for a particular method, you can use a hack to add the `RequestMethodDefinition` method to your command / query. All attributes defined above this method will be used when generating the http method.

```csharp
[HttpPost(Controller = "People")]
public record CreatePersonCommand(string FirstName, string LastName): IRequest<int>
{
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
    private void RequestMethodDefinition()
    {
        throw new NotImplementedException();
    }
}
```

### Whole controller template

You can override whole controller template. `MMLib_TemplateType` property must be set to `Controller`.
