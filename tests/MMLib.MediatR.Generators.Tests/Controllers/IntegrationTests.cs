using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Immutable;
using System.Reflection;
using System.Threading.Tasks;
using MMLib.MediatR.Generators.Controllers;
using VerifyXunit;
using Xunit;

namespace MMLib.MediatR.Generators.Tests.Controllers
{
    [UsesVerify]
    public class IntegrationTests
    {
        private static Compilation CreateCompilation(string source)
            => CSharpCompilation.Create("compilation",
                new[] { CSharpSyntaxTree.ParseText(source) },
                new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));

        [Theory]
        [InlineData("AssemblyWithoutMediatR")]
        [InlineData("AssemblyWithOneController")]
        [InlineData("AssemblyWithMultipleControllers")]
        [InlineData("RequestWithResponseType")]
        [InlineData("QueriesWithDifferentFromType")]
        public Task GeneratorShouldGenerateCorrectClasses(string sourceCodeFile)
        {
            var sourceCode = AssemblyHelper.GetStringFromResourceFileAsync($"{sourceCodeFile}.txt");
            var result = RunGenerator(sourceCode).Results[0].GeneratedSources;

            return Verifier.Verify(result)
                .UseParameters(sourceCodeFile);
        }
        
        [Fact]
        public Task GeneratorShouldReportMissingControllerName()
        {
            var sourceCode = AssemblyHelper.GetStringFromResourceFileAsync($"WithoutControllerName.txt");
            var result = RunGenerator(sourceCode);

            return Verifier.Verify(result);
        }

        private static GeneratorDriverRunResult RunGenerator(string sourceCode)
        {
            var compilation = CreateCompilation(sourceCode);
            GeneratorDriver driver = CSharpGeneratorDriver.Create(new ControllersGenerator());
            driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var _, out var _);

            return driver.GetRunResult();
        }
    }
}