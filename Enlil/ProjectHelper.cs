using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Enlil.Domain;

[assembly: InternalsVisibleTo("Enlil.Tests")]  

namespace Enlil
{
    
    public class ProjectHelper
    {
        private readonly BuildContext startContext;
        private readonly Action<ConventionsSetter> conventions = (_) => { };

        public ProjectHelper(string workFolder, Action<ConventionsSetter> conventions = null)
        {
            this.startContext = new BuildContext() { WorkingDirectory = workFolder};
            this.conventions = conventions ?? this.conventions;
        }

        public string WorkingDirectory => startContext.WorkingDirectory;

        public Task<BuildContext> BuildProjectAssembly()
        {
            var services = setupServices();
            var context = startContext;

            return buildAndGetAssembly(services, context);
        }

        private BuildServices setupServices()
        {
            var services = new BuildServices();
            var setter = new ConventionsSetter();
            conventions(setter);
            setter.SetupServices(services);
            return services;
        }

        private Task<BuildContext> buildAndGetAssembly(BuildServices services, BuildContext context)
        {
            try
            {
                context = services.ProjectFileChooser.FindProjectFile(context);
                context = services.ProjectBuilder.BuildProject(context);
                context = services.ProjectBinaryLoader.LoadAssemblyAndContext(context);
            }
            catch (Exception e)
            {
                context.Error = e;
            }

            return Task.FromResult(context);
        }
    }
}
