using System;
using System.IO;

namespace Musicer.Models.Files
{
    public class UrlConverter
    {
        public UrlConverter(DirectoryInfo baseDirectoryInfo)
        {
            BaseDirectoryInfo = baseDirectoryInfo;
        }

        private DirectoryInfo BaseDirectoryInfo { get; set; }

        /// <summary>
        /// 相対パスと BaseDirectoryInfo プロパティから、FileInfo を生成して取得します。
        /// </summary>
        /// <param name="relativePath">
        /// 相対パスを入力します。
        /// このメソッドは、パラメーターに絶対パスや、不正な文字が含まれたパスが入力されることは想定していません。
        /// </param>
        /// <returns>入力された相対パスが示すパスの FIleInfo を返します。</returns>
        public FileInfo GetFileInfo(string relativePath)
        {
            Uri u1 = new Uri($@"{BaseDirectoryInfo.FullName}\");
            Uri u2 = new Uri(u1, relativePath);
            string absolutePath = u2.LocalPath;
            return new FileInfo(absolutePath);
        }
    }
}