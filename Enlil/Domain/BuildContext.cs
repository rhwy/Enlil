using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Enlil.Domain
{
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
        public static BuildContext Default = new BuildContext();
        
        public static BuildContext operator > (BuildContext startContext, string workingFolder)
        {
            var projectHelper = new ProjectHelper(workingFolder);
            var buildContext = projectHelper.BuildProjectAssembly();
            return  buildContext;
        }
        public static BuildContext operator < (BuildContext startContext, string workingFolder)
        {
            var projectHelper = new ProjectHelper(workingFolder);
            var buildContext = projectHelper.BuildProjectAssembly();
            return buildContext;
        }
    }
}