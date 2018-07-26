using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Enlil.Domain;
using Microsoft.Extensions.DependencyModel;

namespace Enlil.Adapters
{
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