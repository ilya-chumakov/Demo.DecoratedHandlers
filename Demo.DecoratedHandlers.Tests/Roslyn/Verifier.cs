using Demo.DecoratedHandlers.Abstractions;
using Demo.DecoratedHandlers.Gen;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using static Microsoft.CodeAnalysis.Testing.ReferenceAssemblies;

namespace Demo.DecoratedHandlers.Tests.Roslyn;

public static partial class CSharpSourceGeneratorVerifier<TSourceGenerator>
    where TSourceGenerator : IIncrementalGenerator, new()
{
    public class Test : CSharpSourceGeneratorTest<TSourceGenerator, DefaultVerifier>
    {
        public Test()
        {
            ReferenceAssemblies = Net.Net90;
            
            var refs = new[]
            {
                typeof(IGenericHandler<,>).Assembly,
                typeof(PipelineGenerator).Assembly,
                typeof(Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions).Assembly,
            };
            foreach (var asm in refs)
            {
                TestState.AdditionalReferences.Add(MetadataReference.CreateFromFile(asm.Location));
            }
        }

        public LanguageVersion LanguageVersion { get; set; } = LanguageVersion.Default;

        protected override CompilationOptions CreateCompilationOptions()
        {
            var compilationOptions = base.CreateCompilationOptions();
            return compilationOptions.WithSpecificDiagnosticOptions(
                compilationOptions.SpecificDiagnosticOptions.SetItems(CSharpVerifierHelper.NullableWarnings));
        }

        protected override ParseOptions CreateParseOptions()
        {
            return ((CSharpParseOptions)base.CreateParseOptions()).WithLanguageVersion(LanguageVersion);
        }
    }
}