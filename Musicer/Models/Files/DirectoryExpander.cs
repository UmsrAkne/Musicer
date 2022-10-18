namespace Musicer.Models.Files
{
    public static class DirectoryExpander
    {
        public static ExtendFileInfo ExpandDirectories(ExtendFileInfo rootDirectory, ExtendFileInfo targetPath)
        {
            if (targetPath.FileSystemInfo.FullName == rootDirectory.FileSystemInfo.FullName)
            {
                // 目的となるパスがルートディレクトリと同じなら、ルートディレクトリだけ展開して終了
                rootDirectory.IsExpanded = true;
                return null;
            }

            if (!targetPath.FileSystemInfo.FullName.StartsWith(rootDirectory.FileSystemInfo.FullName))
            {
                // 目的となるディレクトリがルートディレクトリの子孫かを確認する。
                // たどり着けない場合(このブロックに入った場合)はこれ以上処理できないので処理を終了する。
                return null;
            }

            var differencePath = targetPath.FileSystemInfo.FullName.Substring(rootDirectory.FileSystemInfo.FullName.Length);
            var directoryNames = differencePath.Split(new[] { "\\" }, System.StringSplitOptions.RemoveEmptyEntries);

            rootDirectory.IsExpanded = true;

            var directory = rootDirectory;

            foreach (var name in directoryNames)
            {
                directory.ChildDirectories.ForEach(d =>
                {
                    if (d.IsDirectory || d.IsM3U)
                    {
                        if (d.Name == name)
                        {
                            d.IsExpanded = true;
                            d.IsSelected = true;
                            directory = d;
                        }
                    }
                });
            }

            return directory;
        }
    }
}