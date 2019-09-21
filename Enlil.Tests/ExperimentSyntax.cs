using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Enlil.Domain;
using Enlil.Tests.SampleAndData;
using NFluent;
using Xunit;
using static Enlil.EnlilHelper;

namespace Enlil.Tests
{
    public partial class ExperimentSyntax
    {
        public class MinimizeFrictionAndSyntax
        {
            private string workFolder = SampleProjectHelper.WorkFolder();
            private BuildContext buildContext;
            public MinimizeFrictionAndSyntax()
            {
                buildContext = workFolder > BuildAssemblyForPath;
            }
            
            [Fact]
            public void
                use_static_helper_to_get_types_for_attribute()
            {
                var types = buildContext > TypesForAttribute("Experiment");

                Check.That(types).IsNotNull();
                Check.That(types).HasSize(1);
            }
            
            [Fact]
            public void
                use_static_helper_to_get_types_for_attribute_on_methods()
            {
                var types = buildContext | MethodTypesForAttribute("Experiment");

                Check.That(types).IsNotNull();
                Check.That(types).HasSize(1);
                
            }
            
            [Fact]
            public void
                use_static_helper_to_get_types_for_attribute_on_methods_and_exec()
            {
                var found = buildContext | MethodTypesForAttribute("Experiment");

                foreach (var element in found)
                {
                    var result = element.Method.Invoke(
                        Activator.CreateInstance(element.Type, null), new[] {"Rui"});
                    Check.That(result).IsEqualTo("<h1>Hello <strong>Rui</strong></h1>");
                }

            }
                
        }
    }
}