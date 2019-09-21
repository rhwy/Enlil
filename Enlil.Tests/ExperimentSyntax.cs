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
                use_static_helper()
            {
                var types = buildContext > TypesForAttribute("Experiment");

                Check.That(types).IsNotNull();
                Check.That(types).HasSize(1);
            }
                
        }
    }
}