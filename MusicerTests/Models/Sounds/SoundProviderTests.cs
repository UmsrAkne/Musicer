namespace Musicer.Models.Sounds.Tests
{
    using System.Collections.Generic;
    using NUnit.Framework;

    public class SoundProviderTests
    {
        [TestCase(true)]
        [TestCase(false)]
        public void 空のリストが入っている場合のテスト(bool isLoopPlay)
        {
            SoundProvider soundProvider = new SoundProvider();
            soundProvider.Source = new List<ISound>();
            soundProvider.LoopPlay = isLoopPlay;

            Assert.IsNull(soundProvider.GetSound(), "中のリストは空なので null が返ってくるはず");
            Assert.IsNull(soundProvider.GetSound(), "中のリストは空なので null が返ってくるはず");
        }

        [Test]
        public void 要素１ループなし()
        {
            SoundProvider soundProvider = new SoundProvider();

            var soundA = new Sound(new System.IO.FileInfo("test.mp3"));

            soundProvider.Source = new List<ISound>() { soundA };
            soundProvider.LoopPlay = false;

            Assert.AreEqual(soundProvider.GetSound(), soundA);
            Assert.IsNull(soundProvider.GetSound(), "ループしない設定なので null が返ってくる");
            Assert.IsNull(soundProvider.GetSound(), "ループしない設定なので null が返ってくる");
        }

        [Test]
        public void 要素１ループあり()
        {
            SoundProvider soundProvider = new SoundProvider();

            var soundA = new Sound(new System.IO.FileInfo("test.mp3"));

            soundProvider.Source = new List<ISound>() { soundA };
            soundProvider.LoopPlay = true;

            Assert.AreEqual(soundProvider.GetSound(), soundA);
            Assert.AreEqual(soundProvider.GetSound(), soundA);
            Assert.AreEqual(soundProvider.GetSound(), soundA);
        }

        [Test]
        public void 要素２ループなし()
        {
            SoundProvider soundProvider = new SoundProvider();

            var soundA = new Sound(new System.IO.FileInfo("testA.mp3"));
            var soundB = new Sound(new System.IO.FileInfo("testB.mp3"));

            soundProvider.Source = new List<ISound>() { soundA, soundB };
            soundProvider.LoopPlay = false;

            Assert.AreEqual(soundProvider.GetSound(), soundA);
            Assert.AreEqual(soundProvider.GetSound(), soundB);
            Assert.IsNull(soundProvider.GetSound());
            Assert.IsNull(soundProvider.GetSound());
        }

        [Test]
        public void 要素２ループあり()
        {
            SoundProvider soundProvider = new SoundProvider();

            var soundA = new Sound(new System.IO.FileInfo("testA.mp3"));
            var soundB = new Sound(new System.IO.FileInfo("testB.mp3"));

            soundProvider.Source = new List<ISound>() { soundA, soundB };
            soundProvider.LoopPlay = true;

            Assert.AreEqual(soundProvider.GetSound(), soundA);
            Assert.AreEqual(soundProvider.GetSound(), soundB);
            Assert.AreEqual(soundProvider.GetSound(), soundA);
            Assert.AreEqual(soundProvider.GetSound(), soundB);
        }

        [Description("GetPreviousSound() のテスト")]
        [TestCase(true)]
        [TestCase(false)]
        public void 空のリストの逆順取得(bool isLoopPlay)
        {
            SoundProvider soundProvider = new SoundProvider();
            soundProvider.Source = new List<ISound>();
            soundProvider.LoopPlay = isLoopPlay;

            Assert.IsNull(soundProvider.GetPreviousSound(), "中のリストは空なので null が返ってくるはず");
            Assert.IsNull(soundProvider.GetPreviousSound(), "中のリストは空なので null が返ってくるはず");
        }

        [Test]
        [Description("GetPreviousSound() のテスト")]
        public void 逆順取得要素１ループなし()
        {
            SoundProvider soundProvider = new SoundProvider();

            var soundA = new Sound(new System.IO.FileInfo("test.mp3"));

            soundProvider.Source = new List<ISound>() { soundA };
            soundProvider.LoopPlay = false;

            Assert.IsNull(soundProvider.GetPreviousSound(), "ループしない設定なので null が返ってくる");
            Assert.IsNull(soundProvider.GetPreviousSound(), "ループしない設定なので null が返ってくる");
        }

        [Test]
        [Description("GetPreviousSound() のテスト")]
        public void 逆順取得要素１ループあり()
        {
            SoundProvider soundProvider = new SoundProvider();

            var soundA = new Sound(new System.IO.FileInfo("test.mp3"));

            soundProvider.Source = new List<ISound>() { soundA };
            soundProvider.LoopPlay = true;

            Assert.AreEqual(soundProvider.GetPreviousSound(), soundA, "ループ取得なので soundA が返ってくる");
            Assert.AreEqual(soundProvider.GetPreviousSound(), soundA, "ループ取得なので soundA が返ってくる");
        }

        [Test]
        [Description("GetPreviousSound() のテスト")]
        public void 逆順取得要素２ループなし()
        {
            SoundProvider soundProvider = new SoundProvider();

            var soundA = new Sound(new System.IO.FileInfo("testA.mp3"));
            var soundB = new Sound(new System.IO.FileInfo("testB.mp3"));

            soundProvider.Source = new List<ISound>() { soundA, soundB };
            soundProvider.LoopPlay = false;

            Assert.IsNull(soundProvider.GetPreviousSound(), "ループしないのでnull");
            Assert.IsNull(soundProvider.GetPreviousSound(), "ループしないのでnull");
        }

        [Test]
        [Description("GetPreviousSound() のテスト")]
        public void 逆順取得要素２ループあり()
        {
            SoundProvider soundProvider = new SoundProvider();

            var soundA = new Sound(new System.IO.FileInfo("testA.mp3"));
            var soundB = new Sound(new System.IO.FileInfo("testB.mp3"));

            soundProvider.Source = new List<ISound>() { soundA, soundB };
            soundProvider.LoopPlay = true;

            Assert.AreEqual(soundProvider.GetPreviousSound(), soundB);
            Assert.AreEqual(soundProvider.GetPreviousSound(), soundA);
            Assert.AreEqual(soundProvider.GetPreviousSound(), soundB);
            Assert.AreEqual(soundProvider.GetPreviousSound(), soundA);
        }
    }
}