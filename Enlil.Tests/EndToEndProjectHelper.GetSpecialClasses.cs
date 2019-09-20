using System;
using System.Reflection;
using System.Threading.Tasks;
using Enlil.Tests.SampleAndData;
using NFluent;
using Xunit;
using static Enlil.AssemblyHelper;
namespace Enlil.Tests
{
    public partial class EndToEndProjectHelper
    {
        public class GetSpecialClasses
        {
            [Theory]
            [InlineData("Experiment",1)]
            [InlineData("I_NOT_EXIST",0)]
            public async Task
                looking_for_an_attibute_on_a_type(string attributeName, int expectedTypesFound)
            {
                var projectHelper = new ProjectHelper(SampleProjectHelper.WorkFolder());
                var buildContext = await projectHelper.BuildProjectAssembly();
                var types = GetTypesByAttributeName(buildContext.ResultingAssembly,attributeName);

                Check.That(types).IsNotNull();
                Check.That(types).HasSize(expectedTypesFound);
            }
            
            

            [Theory]
            [InlineData("Experiment",1)]
            [InlineData("I_NOT_EXIST",0)]
            public async Task
                looking_for_an_attibute_on_a_method(string attributeName, int expectedTypesFound)
            {
                var projectHelper = new ProjectHelper(SampleProjectHelper.WorkFolder());
                var buildContext = await projectHelper.BuildProjectAssembly();
                var types = GetTypesByAttributeNameOnMethod(buildContext.ResultingAssembly,attributeName);

                Check.That(types).IsNotNull(); 
                Check.That(types).HasSize(expectedTypesFound);
            }

           
        }
    }
}