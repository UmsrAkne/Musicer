namespace MusicerTests.Models.Sounds
{
    using Musicer.Models.Sounds;
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

        [Test]
        [Description("サウンドを追加するテスト")]
        public void サウンドを追加するテスト要素３()
        {
            var soundViewer = new SoundViewer();

            soundViewer.Add(new DummySound() { Name = "test1" });
            soundViewer.Add(new DummySound() { Name = "test2" });
            soundViewer.Add(new DummySound() { Name = "test3" });

            Assert.AreEqual(soundViewer.PlayingMusicName, "test2 >>> test3");
        }

        [Test]
        public void 入力したサウンドが終了した際の挙動要素１()
        {
            var soundViewer = new SoundViewer();
            var soundA = new DummySound() { Name = "test1" };

            soundViewer.Add(soundA);
            soundA.DispatchEndedEvent();

            Assert.AreEqual(soundViewer.PlayingMusicName, string.Empty);
        }

        [Test]
        public void 入力したサウンドが終了した際の挙動要素２()
        {
            var soundViewer = new SoundViewer();
            var soundA = new DummySound() { Name = "test1" };

            soundViewer.Add(soundA);
            soundViewer.Add(new DummySound() { Name = "test2" });

            soundA.DispatchEndedEvent();

            Assert.AreEqual(soundViewer.PlayingMusicName, "test2");
        }

        [Test]
        public void リセットのテスト()
        {
            var soundViewer = new SoundViewer();

            soundViewer.Add(new DummySound() { Name = "test1" });
            soundViewer.Add(new DummySound() { Name = "test2" });
            soundViewer.Reset();

            Assert.AreEqual(soundViewer.PlayingMusicName, string.Empty);
        }
    }
}