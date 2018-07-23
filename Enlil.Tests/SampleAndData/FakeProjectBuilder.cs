namespace Enlil.Tests
{
    public class FakeProjectBuilder: IProjectBuilder
    {
        public BuildContext BuildProject(BuildContext context)
        {
            context.ResultingAssemblyFile = "/path/to/resulting/dll";
            context.BinFolder = "/path/to/resulting/dll/bin/folder";
            return context;
        }
    }
}