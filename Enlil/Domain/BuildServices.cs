namespace Enlil.Domain
{
    class BuildServices
    {
        public IProjectFileChooser ProjectFileChooser { get; set; }
        public IProjectBuilder ProjectBuilder { get; set; }
        public IProjectBinaryLoader ProjectBinaryLoader { get; set; }
    }
}