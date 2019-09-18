using System.IO;

namespace Enlil.Tests.SampleAndData
{
    public static class SampleProjectHelper
    {
        public static string WorkFolder()
        {
            var thisProjectFolder = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName;
            var sampleFolder = Path.Combine(thisProjectFolder, "Enlil.Sample");
            return sampleFolder;
        }

        public static string SampleNameSpaceAndClass = "Enlil.Sample.Greetings";
        public static string SampleProjectName = "Enlil.Sample";
        public static string DefaultTargetFramework = "netcoreapp3.0";
    }
}