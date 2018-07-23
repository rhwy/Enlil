using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyModel;

[assembly: InternalsVisibleTo("Bolt.Tests")]  

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

     
    public struct BuildContext
    {
        public Assembly ResultingAssembly { get; set; }
        public string ProjectFile { get; set; }
        public string WorkingDirectory { get; set; }
        
        public Exception Error { get; set; }
        public bool HasError => Error != null;
        public string ResultingAssemblyFile { get; set; }
        public string BinFolder { get; set; }
        public string ProjectName { get; set; }
        public DateTime LastBuildTime { get; set; }
        public long AssemblyLength { get; set; }
    }

    class BuildServices
    {
        public IProjectFileChooser ProjectFileChooser { get; set; }
        public IProjectBuilder ProjectBuilder { get; set; }
        public IProjectBinaryLoader ProjectBinaryLoader { get; set; }
    }
    
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

    public interface IProjectFileChooser 
    {
        BuildContext FindProjectFile(BuildContext context);
    }

    public interface IProjectBuilder
    {
        BuildContext BuildProject(BuildContext context);
    }

    public interface IProjectBinaryLoader
    {
        BuildContext LoadAssemblyAndContext(BuildContext context);
    }
    
    public class DirectoryProjectFileChooser : IProjectFileChooser
    {
        public BuildContext FindProjectFile(BuildContext context)
        {
            context.ProjectFile = Directory.EnumerateFiles(
                    context.WorkingDirectory ?? Directory.GetCurrentDirectory(),
                    "*.*proj")
                .FirstOrDefault();
            
            var file = new FileInfo(context.ProjectFile);
            if (file.Exists && file.Extension.EndsWith("proj"))
            {
                context.ProjectName = file.Name.Substring(0, file.Name.Length - file.Extension.Length);
            }
            else
            {
                context.Error = new FileNotFoundException($"*.*proj file in {context.WorkingDirectory}");
            }
            return context;
        }
    }
    
    public class MsBuildProjectBuilder : IProjectBuilder
    {
        public BuildContext BuildProject(BuildContext context)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "dotnet",
                UseShellExecute = false,
                Arguments = $"msbuild \"{context.ProjectFile}\" /nologo",
                RedirectStandardInput = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
            
            var process =  new Process();
            process.EnableRaisingEvents = true;
            process.StartInfo = psi;
            List<string> libs = new List<string>();
            process.OutputDataReceived += (sender, args) =>
            {
                if (args.Data?.Contains("Error") == true)
                {
                    context.Error = new Exception($"Build error: {Environment.NewLine}{args.Data}");
                    return;
                }
                string line = args.Data;
                if(line?.Contains("->") == true && line?.Contains(context.ProjectName) == true)
                {
                    var libNameInBuildLog = line.Substring(line.IndexOf(" -> ") + 4);
                    libs.Add(libNameInBuildLog);
                }
            };
            process.Start();
            process.BeginOutputReadLine();
            
            while(!process.HasExited) {}
            
            /* */

            process.WaitForExit();
            if (libs.Any())
            {
                context.ResultingAssemblyFile = libs.FirstOrDefault();
                var buildFileInfo = new FileInfo(context.ResultingAssemblyFile);
                context.LastBuildTime = buildFileInfo.LastWriteTime;
                context.BinFolder = buildFileInfo.Directory.FullName;
                context.AssemblyLength = buildFileInfo.Length;
            }
            else
            {
                context.Error = new Exception("output dll not found");
            }
            
            return context;
        }
    }
    
    public class AssemblyBinaryLoader : IProjectBinaryLoader
    {
        public BuildContext LoadAssemblyAndContext(BuildContext context)
        {
            var asm = Assembly.LoadFile(context.ResultingAssemblyFile);
            var dp = DependencyContext.Load(asm);
            var packageDirs =((string)AppDomain.CurrentDomain.GetData("PROBING_DIRECTORIES")).Split(":".ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
            
            AssemblyLoadContext.Default.Resolving += (ctx, name) =>
            {
                var depInfo = dp.RuntimeLibraries.FirstOrDefault(x => x.Name == name.Name);

                if (depInfo.Type=="package")
                {
                    var loockupFolder = packageDirs.First();
                    var packageFolder=depInfo.Path;
                    var fileName=depInfo.RuntimeAssemblyGroups[0].AssetPaths[0];
                    var fullPath = Path.Combine(loockupFolder, packageFolder, fileName);
                    return Assembly.LoadFile(fullPath);
                }

                return null;
            };

            context.ResultingAssembly = asm;
            return context;
        }
    }
}
