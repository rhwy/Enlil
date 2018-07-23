using System;
using System.Threading.Tasks;
using Enlil.Tests.SampleAndData;
using NFluent;
using Xunit;

namespace Enlil.Tests
{
    public class EndToEndProjectHelperTests
    {
        /// <summary>
        /// end to end : I want a helper that is able to give me an assembly
        /// that I can work with from a project folder containing a .csp
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task get_access_to_resulting_assembly_on_a_project_folder()
        {
            var projectHelper = new ProjectHelper(SampleProjectHelper.WorkFolder());
            var resultContext = await projectHelper.BuildProjectAssembly();

            var asm = resultContext.ResultingAssembly;
            var type = asm.GetType("Sample.Greetings");
            var greetings = Activator.CreateInstance(type);
            var toHtml = type.GetMethod("ToHtml");
            var result = toHtml.Invoke(greetings, new [] { "# Hello!"});
            Check.That(result).IsEqualTo($"<h1>Hello!</h1>{Environment.NewLine}");

        }
    }
}