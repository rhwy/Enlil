using System;
using System.Collections.Generic;
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
                Check.That(types.Element).HasSize(1);
            }
            
            [Fact]
            public void
                use_static_helper_to_get_types_for_attribute_on_methods()
            {
                var types = buildContext | MethodTypesForAttribute("Experiment");

                Check.That(types).IsNotNull();
                Check.That(types.Element).HasSize(2);
                
            }
            
            [Fact]
            public void
                use_static_helper_to_get_types_for_attribute_on_methods_and_exec()
            {
                var found = buildContext | MethodTypesForAttribute("Experiment");

                var action = 
                    found
                     | Each<MethodOnType>( e =>
                        {
                        string result = (string)e.Method.Invoke(
                            Activator.CreateInstance(e.Type, null), new object[] {"Rui"});
                        
                        Check.That(result).Contains("Rui");
                        });
            }
            [Fact]
            public void
                use_static_helper_to_get_types_for_attribute_on_methods_and_exec_classic()
            {
                var found = buildContext | MethodTypesForAttribute("Experiment");

                found.Each(
                    e =>
                    {
                        
                        var result = e.InvokeMethod<string>(args: new object[] {"Rui"});
                        
                        if (e.HasMethodAttributeArgument("Name", "one"))
                        {
                            Check.That(result).IsEqualTo("<h1>Hello <strong>Rui</strong></h1>");
                        } else if (e.HasMethodAttributeArgument(name: "Name", "two"))
                        {
                            Check.That(result).IsEqualTo("Olà Rui");
                        }
                        else
                        {
                            throw new Exception("we are supposed to have only these 2 test cases");
                        }
                        
                    });
            }
        }
        
    }
}