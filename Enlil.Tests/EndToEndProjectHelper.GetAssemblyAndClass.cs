using System;
using System.Threading.Tasks;
using Enlil.Tests.SampleAndData;
using NFluent;
using Xunit;

namespace Enlil.Tests
{
    public partial class EndToEndProjectHelper
    {
        public class GetAssemblyAndClass
        {


            /// <summary>
            /// end to end : I want a helper that is able to give me an assembly
            /// that I can work with from a project folder containing a .csp
            /// </summary>
            /// <returns></returns>
            [Fact]
            public void get_access_to_resulting_assembly_on_a_project_folder()
            {
                var projectHelper = new ProjectHelper(SampleProjectHelper.WorkFolder());
                var resultContext = projectHelper.BuildProjectAssembly();

                var asm = resultContext.ResultingAssembly;
                var type = asm.GetType("Enlil.Sample.Greetings");
                var greetings = Activator.CreateInstance(type);
                var sayHello = type.GetMethod("SayHello",new Type[]{});
                var result = sayHello?.Invoke(greetings, new object[] {});
                Check.That(result).IsEqualTo($"Hello World");
            }

            [Fact]
            public void get_access_to_resulting_assembly_on_a_project_folder_with_dependency()
            {
                var projectHelper = new ProjectHelper(SampleProjectHelper.WorkFolder());
                var resultContext = projectHelper.BuildProjectAssembly();

                var asm = resultContext.ResultingAssembly;
                var type = asm.GetType("Enlil.Sample.Greetings");
                var greetings = Activator.CreateInstance(type);
                var sayHello = type.GetMethod("SayHello",new Type[]{typeof(string)});
                var result = sayHello?.Invoke(greetings, new object[] {"Rui"});
                Check.That(result).IsEqualTo($"<h1>Hello <strong>Rui</strong></h1>");
            }
        }
    }
}