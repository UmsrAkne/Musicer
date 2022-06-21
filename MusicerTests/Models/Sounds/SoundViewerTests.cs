namespace Musicer.Models.Sounds.Tests
{
    using NUnit.Framework;

    public class SoundViewerTests
    {
        [Test]
        [Description("サウンドを追加するテスト")]
        public void サウンドを追加するテスト要素１()
        {
            var soundViewer = new SoundViewer();

            soundViewer.Add(new DummySound() { Name = "test1" });

            Assert.AreEqual(soundViewer.PlayingMusicName, "test1");
        }

        [Test]
        [Description("サウンドを追加するテスト")]
        public void サウンドを追加するテスト要素２()
        {
            var soundViewer = new SoundViewer();

            soundViewer.Add(new DummySound() { Name = "test1" });
            soundViewer.Add(new DummySound() { Name = "test2" });

            Assert.AreEqual(soundViewer.PlayingMusicName, "test1 >>> test2");
        }
    }
}