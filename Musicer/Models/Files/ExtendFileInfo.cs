namespace Musicer.Models.Files
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class ExtendFileInfo
    {
        private List<ExtendFileInfo> childDirectories;

        public ExtendFileInfo(string path)
        {
            IsDirectory = File.GetAttributes(path).HasFlag(FileAttributes.Directory);

            if (IsDirectory)
            {
                var d = new DirectoryInfo(path);
                FileSystemInfo = d;
                HasChildDirectory = d.GetDirectories().Length > 0;
            }
            else
            {
                var f = new FileInfo(path);
                IsSoundFile = f.Extension == ".mp3" || f.Extension == ".ogg" || f.Extension == ".wav";
                IsM3U = f.Extension == ".m3u";
                FileSystemInfo = f;
            }
        }

        public FileSystemInfo FileSystemInfo { get; private set; }

        public string Name => FileSystemInfo.Name;

        public bool HasChildDirectory { get; private set; }

        public List<ExtendFileInfo> ChildDirectories
        {
            get
            {
                if (childDirectories != null)
                {
                    return childDirectories;
                }

                if (HasChildDirectory && IsDirectory)
                {
                    var directories = (FileSystemInfo as DirectoryInfo).GetDirectories().ToList();
                    childDirectories = directories.Select(d => new ExtendFileInfo(d.FullName)).ToList();
                    return childDirectories;
                }
                else
                {
                    childDirectories = new List<ExtendFileInfo>();
                    return childDirectories;
                }
            }
        }

        public bool IsDirectory { get; private set; }

        public bool IsSoundFile { get; private set; }

        public bool IsM3U { get; private set; }
    }
}
