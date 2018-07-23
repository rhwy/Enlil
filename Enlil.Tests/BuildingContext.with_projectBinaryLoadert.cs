using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Enlil;
using Enlil.Tests.SampleAndData;
using NFluent;
using Xunit;

namespace Enlil.Tests
{
    public partial class BuildingContext
    {
        public class with_projectFileChooser
        {

            [Fact]
            public async Task ensure_working_directoty_is_set()
            {

                var projectHelper = new ProjectHelper(SampleProjectHelper.WorkFolder());

                Check.That(projectHelper.WorkingDirectory).IsEqualTo(SampleProjectHelper.WorkFolder());
            }

            [Fact]
            public async Task ensure_projectFileChooser_is_called_in_build()
            {

                Action<ConventionsSetter> setconventions = overload =>
                {
                    overload.SetProjectFileChooser(new FakeProjectFileChooser());
                };
                var projectHelper = new ProjectHelper(SampleProjectHelper.WorkFolder(), setconventions);
                var resultContext = await projectHelper.BuildProjectAssembly();

                Check.That(resultContext.ProjectFile).IsEqualTo("/path/to/csproj");
            }

            [Fact]
            public async Task ensure_default_projectFileChooser_take_the_right_File()
            {
                var projectHelper = new ProjectHelper(SampleProjectHelper.WorkFolder());
                var resultContext = await projectHelper.BuildProjectAssembly();

                Check.That(resultContext).HasNoErrors();
                Check.That(resultContext.ProjectFile)
                    .IsEqualTo(Path.Combine(SampleProjectHelper.WorkFolder(), "Sample.csproj"));
                Check.That(resultContext.ProjectName).IsEqualTo("Sample");
            }
        }
    }


/* EXPERIMENT WITH FILEINFO : (try file provider experiments once ready)
 ---------------------------- 
    public class InMemoryFileProvider : IFileProvider
    {
        private readonly string _root;
        private readonly IList<(string name, string path, string content)>  source;
        private readonly IEnumerable<IFileInfo> list;
        
        public InMemoryFileProvider(string root, IList<(string name, string path, string content)> contents)
        {
            _root = root;
            source = contents;
            list = contents.Select(x => new InMemoryFileInfo(x.content, x.path, x.name));
        }

        public IFileInfo GetFileInfo(string subpath)
            => list.FirstOrDefault(x => x.PhysicalPath == Path.Combine(_root,subpath));

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            throw new NotImplementedException();
        }

        public IChangeToken Watch(string filter)
        {
            throw new NotImplementedException();
        }
    }

    public class InMemoryFileInfo : IFileInfo
    {
        private readonly string _content;

        public InMemoryFileInfo(string content, string physicalPath,
            string name = null,
            DateTimeOffset lastModified = default(DateTimeOffset), bool exists = true)
        {
            _content = content;
            PhysicalPath = physicalPath;
            Name = name ?? physicalPath.Split('/', '\\').Last();
            Exists = exists;
            LastModified = lastModified == default(DateTimeOffset) ? DateTimeOffset.Now : lastModified;
            IsDirectory = false;
        }
        public Stream CreateReadStream()
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(_content));
        }

        public bool Exists { get; }
        public long Length { get; }
        public string PhysicalPath { get; }
        public string Name { get; }
        public DateTimeOffset LastModified { get; }
        public bool IsDirectory { get; }
    }

    public class InMemoryDirectoryContents : IDirectoryContents
    {
        private readonly IEnumerable<IFileInfo> _list;

        public InMemoryDirectoryContents(IEnumerable<IFileInfo> list = null)
        {
            _list = list;
            Exists = list != null;
        }

        public IEnumerator<IFileInfo> GetEnumerator()
            => _list?.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _list?.GetEnumerator();

        public bool Exists { get; }
    }
    */
}
