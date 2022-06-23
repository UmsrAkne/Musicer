namespace Musicer.Models.Files
{
    using System.IO;

    public static class DirectoryExpander
    {
        public static bool ExpandDirectories(ExtendFileInfo rootDirectory, DirectoryInfo targetPath)
        {
            if (targetPath.FullName == rootDirectory.FileSystemInfo.FullName)
            {
                // 目的となるパスがルートディレクトリと同じなら、ルートディレクトリだけ展開して終了
                rootDirectory.IsExpanded = true;
                return false;
            }

            if (!targetPath.FullName.StartsWith(rootDirectory.FileSystemInfo.FullName))
            {
                // 目的となるディレクトリがルートディレクトリの子孫かを確認する。
                // たどり着けない場合(このブロックに入った場合)はこれ以上処理できないので処理を終了する。
                return false;
            }

            var differencePath = targetPath.FullName.Substring(rootDirectory.FileSystemInfo.FullName.Length);
            var directoryNames = differencePath.Split(new string[] { "\\" }, System.StringSplitOptions.RemoveEmptyEntries);

            rootDirectory.IsExpanded = true;

            var directory = rootDirectory;

            foreach (var name in directoryNames)
            {
                directory.ChildDirectories.ForEach(d =>
                {
                    if (d.IsDirectory && d.Name == name)
                    {
                        d.IsExpanded = true;
                        directory = d;
                    }
                });
            }

            return true;
        }
    }
}
