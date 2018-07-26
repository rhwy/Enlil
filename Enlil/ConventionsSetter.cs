using Enlil.Adapters;
using Enlil.Domain;

namespace Enlil
{
    public class ConventionsSetter
    {
        private IProjectFileChooser projectFileChooser;
        private IProjectBuilder projectBuilder;
        private IProjectBinaryLoader projectBinaryLoader;
        
        internal void SetupServices(BuildServices services)
        {
            services.ProjectFileChooser = projectFileChooser ?? new DirectoryProjectFileChooser();
            services.ProjectBuilder = projectBuilder ?? new MsBuildProjectBuilder();
            services.ProjectBinaryLoader = projectBinaryLoader ?? new AssemblyBinaryLoader();
        }
        
        public void SetProjectFileChooser(IProjectFileChooser projectFileChooser)
        {
            this.projectFileChooser = projectFileChooser;
        }

        public void SetProjectBuilder(IProjectBuilder projectBuilder)
        {
            this.projectBuilder = projectBuilder;
        }

        public void SetProjectBinaryLoader(IProjectBinaryLoader projectBinaryLoader)
        {
            this.projectBinaryLoader = projectBinaryLoader;
        }
    }
}