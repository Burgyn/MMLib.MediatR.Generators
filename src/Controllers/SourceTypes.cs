using System;

namespace MMLib.MediatR.Generators.Controllers
{
    /// <summary>
    /// Specifies where the parameter is taken from.
    /// </summary>
    public enum From
    {
        /// <summary>
        /// The command / query will not be used as a parameter of the Http action.
        /// </summary>
        Ignore,

        /// <summary>
        /// Specifies that a parameter should be bound using route-data from the current request.
        /// </summary>
        Route,

        /// <summary>
        /// Specifies that a parameter should be bound using the request query string.
        /// </summary>
        Query,

        /// <summary>
        /// Specifies that a parameter property should be bound using the request body.
        /// </summary>
        Body,

        /// <summary>
        /// Specifies that a parameter should be bound using the request headers.
        /// </summary>
        Header,

        /// <summary>
        /// Specifies that a parameter should be bound using form-data in the request body.
        /// </summary>
        Form,

        /// <summary>
        /// Specifies that an parameter should be bound using the request services.
        /// </summary>
        Services
    }

    /// <summary>
    /// Base Http mehtod attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public abstract class HttpMethodAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpGetAttribute"/> class.
        /// </summary>
        protected HttpMethodAttribute() : this(string.Empty) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpGetAttribute"/> class.
        /// </summary>
        /// <param name="template">The route template.</param>
        protected HttpMethodAttribute(string template)
        {
            Template = template;
        }

        /// <summary>
        /// Gets or sets the name of generated controller.
        /// </summary>
        public string Controller { get; init; }

        /// <summary>
        /// Http action name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the route template.
        /// </summary>
        public string Template { get; }

        /// <summary>
        /// Specifies where the parameter is taken from.
        /// </summary>
        public From From { get; init; }

        /// <summary>
        /// Gets or sets the type of the response.
        /// </summary>
        public Type ResponseType { get; init; }
    }

    /// <summary>
    /// Identifies the query from which the HTTP Get action method will be generated.
    /// </summary>
    public class HttpGetAttribute : HttpMethodAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpGetAttribute"/> class.
        /// </summary>
        public HttpGetAttribute() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpGetAttribute"/> class.
        /// </summary>
        /// <param name="template">The route template.</param>
        public HttpGetAttribute(string template) : base(template) { }
    }

    /// <summary>
    /// Identifies the command from which the HTTP Post action method will be generated.
    /// </summary>
    public class HttpPostAttribute : HttpMethodAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpPostAttribute"/> class.
        /// </summary>
        public HttpPostAttribute() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpPostAttribute"/> class.
        /// </summary>
        /// <param name="template">The route template. May not be null.</param>
        public HttpPostAttribute(string template) : base(template) { }
    }

    /// <summary>
    /// Identifies the command from which the HTTP Put action method will be generated.
    /// </summary>
    public class HttpPutAttribute : HttpMethodAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpPutAttribute"/> class.
        /// </summary>
        public HttpPutAttribute() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpPutAttribute"/> class.
        /// </summary>
        /// <param name="template">The route template. May not be null.</param>
        public HttpPutAttribute(string template) : base(template) { }
    }

    /// <summary>
    /// Identifies the command from which the HTTP Delete action method will be generated.
    /// </summary>
    public class HttpDeleteAttribute : HttpMethodAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpPutAttribute"/> class.
        /// </summary>
        public HttpDeleteAttribute() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpPutAttribute"/> class.
        /// </summary>
        /// <param name="template">The route template. May not be null.</param>
        public HttpDeleteAttribute(string template) : base(template) { }
    }

    /// <summary>
    /// Additional parameters that are added as parameters of the http method and used to post initiate the command.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AdditionalParametersAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdditionalParametersAttribute"/> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public AdditionalParametersAttribute(params string[] parameters)
        {
            Parameters = parameters;
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        public string[] Parameters { get; }
    }
}
