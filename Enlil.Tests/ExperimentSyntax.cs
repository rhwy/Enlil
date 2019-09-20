using System.Threading.Tasks;
using Enlil.Tests.SampleAndData;
using Xunit;
using static Enlil.EnlilHelper;

namespace Enlil.Tests
{
    public partial class ExperimentSyntax
    {
        public class MinimizeFrictionAndSyntax
        {
            private string workFolder = SampleProjectHelper.WorkFolder();
            [Fact]
            public async Task
                use_static_helper()
            {
                var buildContext = BUILD_ASSEMBLY 
                                   > workFolder;

                //var projectHelper = new ProjectHelper(SampleProjectHelper.WorkFolder());
                //var buildContext = await projectHelper.BuildProjectAssembly();
                //var types = GetTypesByAttributeName(buildContext.ResultingAssembly,attributeName);

            }
                
        }
    }
}