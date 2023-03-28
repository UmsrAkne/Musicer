using System.IO;
using Musicer.Models.Files;
using NUnit.Framework;

namespace MusicerTests.Models.Files
{
    [TestFixture]
    public class UrlConverterTest
    {
        [Test]
        public void GetFileInfoTest()
        {
            var converter = new UrlConverter(new DirectoryInfo(@"C:\testDirectory\"));

            Assert.AreEqual(
                @"C:\nextTestDirectory\testFile",
                converter.GetFileInfo(@"..\nextTestDirectory\testFile").FullName);

            // １階層深くなっても動くかテスト
            var converter2 = new UrlConverter(new DirectoryInfo(@"C:\test\testDirectory\"));

            Assert.AreEqual(
                @"C:\test\nextDirectory\testFile",
                converter2.GetFileInfo(@"..\nextDirectory\testFile").FullName);
        }

        [Test]
        public void 絶対パス入力時のテスト()
        {
            var converter = new UrlConverter(new DirectoryInfo(@"C:\testDirectory\"));

            Assert.AreEqual(
                @"C:\nextTestDirectory\testFile",
                converter.GetFileInfo(@"C:\nextTestDirectory\testFile").FullName);
        }
    }
}