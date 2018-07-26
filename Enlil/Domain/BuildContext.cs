using System;
using System.Reflection;

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
    }
}