using System;
using System.Collections;
using System.Collections.Generic;
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
        
        public static BuildContext operator > (string workingFolder,BuildContext startContext)
        {
            var projectHelper = new ProjectHelper(workingFolder);
            var buildContext = projectHelper.BuildProjectAssembly();
            return  buildContext;
        }
        public static BuildContext operator < (string workingFolder,BuildContext startContext)
        {
            var projectHelper = new ProjectHelper(workingFolder);
            var buildContext = projectHelper.BuildProjectAssembly();
            return buildContext;
        }
        public static IEnumerable<Type> operator > (BuildContext buildContext, TypeFilter filter)
        {
            return filter(buildContext);
        }

        public static IEnumerable<Type> operator <(BuildContext buildContext, TypeFilter filter)
            => filter(buildContext);

        public static IEnumerable<MethodOnType> operator |(BuildContext buildContext, MethodFilter filter)
            => filter(buildContext);

    }
}