using System;
using System.IO;
using System.Threading.Tasks;
using Enlil.Tests.SampleAndData;
using NFluent;
using Xunit;
using Xunit.Abstractions;

namespace Enlil.Tests
{
    public partial class BuildingContext
    {
        public class with_projectBuilder
        {
            private readonly ITestOutputHelper _outputHelper;

            public with_projectBuilder(ITestOutputHelper outputHelper)
            {
                _outputHelper = outputHelper;
            }
            [Fact]
            public void
                ensure_projectBuilder_is_called()
            {
                Action<ConventionsSetter> setconventions = overload =>
                {
                    overload.SetProjectBuilder(new FakeProjectBuilder());
                };
                var projectHelper = new ProjectHelper(SampleProjectHelper.WorkFolder(), setconventions);
                var resultContext = projectHelper.BuildProjectAssembly();

                Check.That(resultContext.ResultingAssemblyFile).IsEqualTo("/path/to/resulting/dll");
                Check.That(resultContext.BinFolder).IsEqualTo("/path/to/resulting/dll/bin/folder");

            }

            [Fact]
            public void
                ensure_default_project_builder_builds_the_project_when_it_exists()
            {
                var projectHelper = new ProjectHelper(SampleProjectHelper.WorkFolder());
                var resultContext = projectHelper.BuildProjectAssembly();

                var currentTarget = new DirectoryInfo(Directory.GetCurrentDirectory()).Name;
                var expectedBuildFile =
                    Path.Combine(SampleProjectHelper.WorkFolder(), "bin/Debug",currentTarget,$"{SampleProjectHelper.SampleProjectName}.dll");
                var expectedBinFolder =
                    Path.Combine(SampleProjectHelper.WorkFolder(), "bin/Debug",currentTarget);

                Check.That(resultContext.ResultingAssemblyFile).IsEqualTo(expectedBuildFile);
                Check.That(resultContext.BinFolder).IsEqualTo(expectedBinFolder);
                Check.That(resultContext.AssemblyLength).IsStrictlyGreaterThan(0);

            }
        }
    }
    
}