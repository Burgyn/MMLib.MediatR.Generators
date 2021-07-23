using System;

namespace MMLib.MediatR.Generators.Controllers
{
    /// <summary>
    /// Add <see cref="Controller"/> property to base <seealso cref="Microsoft.AspNetCore.Mvc.HttpGetAttribute" /> attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class HttpGetAttribute : Microsoft.AspNetCore.Mvc.HttpGetAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpGetAttribute"/> class.
        /// </summary>
        public HttpGetAttribute() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpGetAttribute"/> class.
        /// </summary>
        /// <param name="template">The route template. May not be null.</param>
        public HttpGetAttribute(string template) : base(template) { }

        /// <summary>
        /// Gets or sets the name of generated controller.
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// Gets or sets the method signature template name for generator.
        /// </summary>
        public string MethodSignatureTemplate { get; set; }

        /// <summary>
        /// Gets or sets the method signature template name for generator.
        /// </summary>
        public string MethodBodyTemplate { get; set; }
    }

    /// <summary>
    /// Add <see cref="Controller"/> property to base <seealso cref="Microsoft.AspNetCore.Mvc.HttpPostAttribute" /> attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class HttpPostAttribute : Microsoft.AspNetCore.Mvc.HttpPostAttribute
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

        /// <summary>
        /// Gets or sets the name of generated controller.
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// Gets or sets the method signature template name for generator.
        /// </summary>
        public string MethodSignatureTemplate { get; set; }

        /// <summary>
        /// Gets or sets the method signature template name for generator.
        /// </summary>
        public string MethodBodyTemplate { get; set; }
    }

    /// <summary>
    /// Add <see cref="Controller"/> property to base <seealso cref="Microsoft.AspNetCore.Mvc.HttpPutAttribute" /> attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class HttpPutAttribute : Microsoft.AspNetCore.Mvc.HttpPutAttribute
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

        /// <summary>
        /// Gets or sets the name of generated controller.
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// Gets or sets the method signature template name for generator.
        /// </summary>
        public string MethodSignatureTemplate { get; set; }

        /// <summary>
        /// Gets or sets the method signature template name for generator.
        /// </summary>
        public string MethodBodyTemplate { get; set; }
    }

    /// <summary>
    /// Add <see cref="Controller"/> property to base <seealso cref="Microsoft.AspNetCore.Mvc.HttpDeleteAttribute" /> attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class HttpDeleteAttribute : Microsoft.AspNetCore.Mvc.HttpDeleteAttribute
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

        /// <summary>
        /// Gets or sets the name of generated controller.
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// Gets or sets the method signature template name for generator.
        /// </summary>
        public string MethodSignatureTemplate { get; set; }

        /// <summary>
        /// Gets or sets the method signature template name for generator.
        /// </summary>
        public string MethodBodyTemplate { get; set; }
    }

    /// <summary>
    /// Allows you to add a <seealso cref="Microsoft.AspNetCore.Mvc.FromHeaderAttribute" /> to a class as well.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class FromHeaderAttribute : Microsoft.AspNetCore.Mvc.FromHeaderAttribute
    {
    }

    /// <summary>
    /// Allows you to add a <seealso cref="Microsoft.AspNetCore.Mvc.FromQueryAttribute" /> to a class as well.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class FromQueryAttribute : Microsoft.AspNetCore.Mvc.FromQueryAttribute
    {
    }

    /// <summary>
    /// Allows you to add a <seealso cref="Microsoft.AspNetCore.Mvc.FromRouteAttribute" /> to a class as well.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class FromRouteAttribute : Microsoft.AspNetCore.Mvc.FromRouteAttribute
    {
    }

    /// <summary>
    /// The command / query marked with this attribute will not be used as a parameter of the http method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class IgnoreAsParameterAttribute : Attribute
    {
    }

    /// <summary>
    /// Additional parameters that are added as parameters of the http method and used to post initiate the command.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage( AttributeTargets.Class, AllowMultiple = false)]
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
