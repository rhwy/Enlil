using System.IO;
using System.Linq;
using Enlil.Domain;

namespace Enlil.Adapters
{
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
}