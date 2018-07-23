using System;
using System.IO;
using System.Threading.Tasks;
using Enlil.Tests.SampleAndData;
using NFluent;
using Xunit;

namespace Enlil.Tests
{
    public partial class BuildingContext
    {
        public class with_projectBuilder
        {
            [Fact]
            public async Task
                ensure_projectBuilder_is_called()
            {
                Action<ConventionsSetter> setconventions = overload =>
                {
                    overload.SetProjectBuilder(new FakeProjectBuilder());
                };
                var projectHelper = new ProjectHelper(SampleProjectHelper.WorkFolder(), setconventions);
                var resultContext = await projectHelper.BuildProjectAssembly();

                Check.That(resultContext.ResultingAssemblyFile).IsEqualTo("/path/to/resulting/dll");
                Check.That(resultContext.BinFolder).IsEqualTo("/path/to/resulting/dll/bin/folder");

            }

            [Fact]
            public async Task
                ensure_default_project_builder_builds_the_project_when_it_exists()
            {
                var projectHelper = new ProjectHelper(SampleProjectHelper.WorkFolder());
                var resultContext = await projectHelper.BuildProjectAssembly();

                var expectedBuildFile =
                    Path.Combine(SampleProjectHelper.WorkFolder(), "bin/Debug/netstandard2.0/Sample.dll");
                var expectedBinFolder =
                    Path.Combine(SampleProjectHelper.WorkFolder(), "bin/Debug/netstandard2.0");

                Check.That(resultContext.ResultingAssemblyFile).IsEqualTo(expectedBuildFile);
                Check.That(resultContext.BinFolder).IsEqualTo(expectedBinFolder);
                Check.That(resultContext.AssemblyLength).IsStrictlyGreaterThan(0);

            }
        }
    }
    
}