namespace Enlil.Domain
{
    public interface IProjectBinaryLoader
    {
        BuildContext LoadAssemblyAndContext(BuildContext context);
    }
}