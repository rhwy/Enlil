using System.IO;

namespace Enlil.Tests.SampleAndData
{
    public static class SampleProjectHelper
    {
        public static string WorkFolder()
        {
            var thisProjectFolder = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            var sampleFolder = Path.Combine(thisProjectFolder, "Sample");
            return sampleFolder;
        }
    }
}