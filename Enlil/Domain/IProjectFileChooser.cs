namespace Enlil.Domain
{
    public interface IProjectFileChooser 
    {
        BuildContext FindProjectFile(BuildContext context);
    }
}