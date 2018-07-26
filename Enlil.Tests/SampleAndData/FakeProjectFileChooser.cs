using Enlil.Domain;

namespace Enlil.Tests
{
    public class FakeProjectFileChooser : IProjectFileChooser
    {
        public BuildContext FindProjectFile(BuildContext context)
        {
            context.ProjectFile = "/path/to/csproj";
            return context;
        }
    }
}