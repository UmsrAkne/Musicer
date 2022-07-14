namespace Musicer.Models.Files.Tests
{
    using System.IO;
    using NUnit.Framework;

    public class DirectoryExpanderTests
    {
        [Test]
        public void ExpandDirectoriesTest()
        {
            var testDirectory1 = new DirectoryInfo("test");
            testDirectory1.Create();

            var testDirectory2 = new DirectoryInfo("test\\test2");
            testDirectory2.Create();

            var testDirectory22 = new DirectoryInfo("test\\test22");
            testDirectory22.Create();

            var testDirectory3 = new DirectoryInfo("test\\test2\\test3");
            testDirectory3.Create();

            var rootDirectory = new ExtendFileInfo("test");
            var targetDirectory = new ExtendFileInfo("test\\test2\\test3");
            DirectoryExpander.ExpandDirectories(rootDirectory, targetDirectory);

            Assert.AreEqual(rootDirectory.ChildDirectories[0].Name, "test2");
            Assert.IsTrue(rootDirectory.ChildDirectories[0].IsExpanded);

            Assert.AreEqual(rootDirectory.ChildDirectories[1].Name, "test22");
            Assert.IsFalse(rootDirectory.ChildDirectories[1].IsExpanded);

            Assert.AreEqual(rootDirectory.ChildDirectories[0].ChildDirectories[0].Name, "test3");
            Assert.IsTrue(rootDirectory.ChildDirectories[0].ChildDirectories[0].IsExpanded);

            testDirectory3.Delete();
            testDirectory2.Delete();
            testDirectory22.Delete();
            testDirectory1.Delete();
        }
    }
}