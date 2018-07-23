using System;
using System.Threading.Tasks;
using Enlil.Tests.SampleAndData;
using NFluent;
using Xunit;

namespace Enlil.Tests
{
    public partial class BuildingContext
    {
        public class with_projectBinaryLoader
        {
            [Fact]
            public async Task
                ensure_projectBinaryLoader_is_called()
            {
                Action<ConventionsSetter> setconventions = overload =>
                {
                    overload.SetProjectBuilder(new FakeProjectBuilder());
                    overload.SetProjectBinaryLoader(new FakeProjectBinaryLoader());
                };
                var projectHelper = new ProjectHelper(SampleProjectHelper.WorkFolder(), setconventions);
                var resultContext = await projectHelper.BuildProjectAssembly();

                Check.That(resultContext).HasNoErrors();
                Check.That(resultContext.AssemblyLength).IsEqualTo(1000);
            }

            [Fact]
            public async Task ensure_default_projectBinaryLoader_loads_assembly()
            {
                var projectHelper = new ProjectHelper(SampleProjectHelper.WorkFolder());
                var resultContext = await projectHelper.BuildProjectAssembly();

                Check.That(resultContext).HasNoErrors();
                Check.That(resultContext.AssemblyLength).IsStrictlyGreaterThan(1000);
                Check.That(resultContext.ResultingAssembly).IsNotNull();
            }

            [Fact]
            public async Task ensure_default_projectBinaryLoader_loads_assembly_and_get_types()
            {
                var projectHelper = new ProjectHelper(SampleProjectHelper.WorkFolder());
                var resultContext = await projectHelper.BuildProjectAssembly();

                Check.That(resultContext).HasNoErrors();

                var asm = resultContext.ResultingAssembly;
                var type = asm.GetType("Sample.Greetings");

                Check.That(type).IsNotNull();

                var greetings = Activator.CreateInstance(type);
                var sayHello = type.GetMethod("SayHello");
                var result = sayHello.Invoke(greetings, null);
                Check.That(result).IsEqualTo("Hello World");

            }

            [Fact]
            public async Task ensure_default_projectBinaryLoader_loads_assembly_and_get_types_with_dependencies()
            {
                var projectHelper = new ProjectHelper(SampleProjectHelper.WorkFolder());
                var resultContext = await projectHelper.BuildProjectAssembly();

                var asm = resultContext.ResultingAssembly;
                var type = asm.GetType("Sample.Greetings");
                var greetings = Activator.CreateInstance(type);
                var toHtml = type.GetMethod("ToHtml");
                var result = toHtml.Invoke(greetings, new[] {"# Hello!"});
                Check.That(result).IsEqualTo($"<h1>Hello!</h1>{Environment.NewLine}");

            }
        }
    }
}