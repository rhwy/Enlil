using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Enlil.Domain;

namespace Enlil.Adapters
{
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
}