namespace Musicer.Models.Files
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Prism.Mvvm;

    public class ExtendFileInfo : BindableBase
    {
        private List<ExtendFileInfo> childDirectories;

        private bool isExpanded;
        private bool isSelected;

        public ExtendFileInfo(string path)
        {
            IsDirectory = File.GetAttributes(path).HasFlag(FileAttributes.Directory);

            if (IsDirectory)
            {
                var d = new DirectoryInfo(path);
                FileSystemInfo = d;
                HasChildDirectory = d.GetDirectories().Length > 0 || d.GetFiles().Any(f => f.Extension == ".m3u");
            }
            else
            {
                var f = new FileInfo(path);
                IsSoundFile = IsSoundFileExtension(f.Extension);
                IsM3U = IsM3UExtension(f.Extension);
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
                    var directories = (FileSystemInfo as DirectoryInfo)?.GetDirectories().ToList();

                    // ReSharper disable once InconsistentNaming
                    var m3us = (FileSystemInfo as DirectoryInfo)?.GetFiles().Where(f => IsM3UExtension(f.Extension)).ToList();

                    childDirectories = directories.Select(d => new ExtendFileInfo(d.FullName)).ToList();
                    childDirectories.AddRange(m3us.Select(f => new ExtendFileInfo(f.FullName)).ToList());

                    return childDirectories;
                }
                else
                {
                    childDirectories = new List<ExtendFileInfo>();
                    return childDirectories;
                }
            }
        }

        public bool IsExpanded { get => isExpanded; set => SetProperty(ref isExpanded, value); }

        public bool IsSelected { get => isSelected; set => SetProperty(ref isSelected, value); }

        public bool HasSoundFile
        {
            get
            {
                if (IsDirectory)
                {
                    var innerFiles = (FileSystemInfo as DirectoryInfo)?.GetFiles();
                    return innerFiles != null && innerFiles.Any(f => IsSoundFileExtension(f.Extension));
                }

                if (IsM3U)
                {
                    return GetPlayListFromM3U(File.ReadAllLines(FileSystemInfo.FullName)).Count > 0;
                }

                return false;
            }
        }

        public bool IsDirectory { get; private set; }

        public bool IsSoundFile { get; private set; }

        public bool IsM3U { get; private set; }

        /// <summary>
        /// m3uファイル(行毎にパスが記述されたファイル)のテキストを FileInfo のリストにして返します。
        /// </summary>
        /// <param name="m3uText">m3u ファイルを System.IO.File.ReadAllLines() で読み込んだ値を入力する。</param>
        /// <returns>サウンドファイルの情報をもった ExtendFileInfo のリストを返します。</returns>
        // ReSharper disable once InconsistentNaming
        public List<ExtendFileInfo> GetPlayListFromM3U(string[] m3uText)
        {
            return m3uText.Where(line => !line.StartsWith("#") && File.Exists(line))
                          .Select(line => new ExtendFileInfo(line))
                          .Where(fi => fi.IsSoundFile)
                          .ToList();
        }

        private bool IsSoundFileExtension(string extension)
        {
            return extension == ".mp3" || extension == ".ogg" || extension == ".wav";
        }

        private bool IsM3UExtension(string extension)
        {
            return extension == ".m3u" || extension == ".m3u8" || extension == ".m3u16";
        }
    }
}