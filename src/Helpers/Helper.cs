using System;

namespace MMLib.MediatR.Generators.Helpers
{
    internal static class Helper
    {
        public static string GetAttributeName<TAttribute>() where TAttribute : Attribute
            => typeof(TAttribute).Name.Replace("Attribute", string.Empty);
    }
}
