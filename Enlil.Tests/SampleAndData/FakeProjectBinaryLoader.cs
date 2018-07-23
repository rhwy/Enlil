using System.Reflection;

namespace Enlil.Tests
{
    public class FakeProjectBinaryLoader : IProjectBinaryLoader
    {
        public BuildContext LoadAssemblyAndContext(BuildContext context)
        {
            context.AssemblyLength = 1000;
            context.ResultingAssembly = Assembly.GetAssembly(typeof(FakeProjectBinaryLoader));

            return context;
        }
    }
}