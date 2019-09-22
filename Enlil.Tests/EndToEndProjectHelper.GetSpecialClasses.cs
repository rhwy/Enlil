using System;
using System.Collections;
using System.Collections.Generic;
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
            public void
                looking_for_an_attibute_on_a_type(string attributeName, int expectedTypesFound)
            {
                var projectHelper = new ProjectHelper(SampleProjectHelper.WorkFolder());
                var buildContext = projectHelper.BuildProjectAssembly();
                var types = GetTypesByAttributeName(buildContext.ResultingAssembly,attributeName);

                Check.That(types).IsNotNull();
                Check.That(types).HasSize(expectedTypesFound);
            }
            
            

            [Theory]
            [InlineData("Experiment",2)]
            [InlineData("I_NOT_EXIST",0)]
            public void
                looking_for_an_attibute_on_a_method(string attributeName, int expectedTypesFound)
            {
                var projectHelper = new ProjectHelper(SampleProjectHelper.WorkFolder());
                var buildContext = projectHelper.BuildProjectAssembly();
                var types = GetTypesByAttributeNameOnMethod(buildContext.ResultingAssembly,attributeName);

                Check.That(types).IsNotNull();
                Check.That(types).InheritsFrom<IEnumerable<MethodOnType>>();
                Check.That(types).HasSize(expectedTypesFound);
            }

            [Theory]
            [InlineData("IGreetings",1)]
            [InlineData("I_NOT_EXIST",0)]
            public void
                looking_for_a_class_implementing_interface(string interfaceName, int expectedTypesFound)
            {
                var projectHelper = new ProjectHelper(SampleProjectHelper.WorkFolder());
                var buildContext = projectHelper.BuildProjectAssembly();
                var types = GetTypesImplementingInterface(buildContext.ResultingAssembly,interfaceName);

                Check.That(types).IsNotNull();
                Check.That(types).InheritsFrom<IEnumerable<Type>>();
                Check.That(types).HasSize(expectedTypesFound);
            }

        }
    }
}