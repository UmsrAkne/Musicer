namespace MusicerTests.Models.Sounds
{
    using System.Collections.Generic;
    using Musicer.Models.Sounds;
    using NUnit.Framework;

    public class SoundProviderTests
    {
        [TestCase(true)]
        [TestCase(false)]
        public void 空のリストが入っている場合のテスト(bool isLoopPlay)
        {
            SoundProvider soundProvider = new SoundProvider
            {
                Source = new List<ISound>(),
                LoopPlay = isLoopPlay,
            };

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
            SoundProvider soundProvider = new SoundProvider
            {
                Source = new List<ISound>(),
                LoopPlay = isLoopPlay,
            };

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

            Assert.AreEqual(soundProvider.GetPreviousSound().Name, soundB.Name);
            Assert.AreEqual(soundProvider.GetPreviousSound().Name, soundA.Name);
            Assert.AreEqual(soundProvider.GetPreviousSound().Name, soundB.Name);
            Assert.AreEqual(soundProvider.GetPreviousSound().Name, soundA.Name);
        }

        [Test]
        public void スキップありのときの取得()
        {
            var soundProvider = new SoundProvider();
            soundProvider.Source = new List<ISound>()
            {
                new DummySound() { Name = "A.mp3", },
                new DummySound() { Name = "B.mp3", IsSkipped = true, },
                new DummySound() { Name = "C.mp3", },
            };

            soundProvider.LoopPlay = true;
            Assert.AreEqual(soundProvider.GetSound().Name, "A.mp3");
            Assert.AreEqual(soundProvider.GetSound().Name, "C.mp3");
        }

        [Test]
        [Description("GetSound() のテスト")]
        public void 途中のインデックスから再生()
        {
            SoundProvider soundProvider = new SoundProvider();

            var soundA = new Sound(new System.IO.FileInfo("testA.mp3"));
            var soundB = new Sound(new System.IO.FileInfo("testB.mp3"));
            var soundC = new Sound(new System.IO.FileInfo("testB.mp3"));

            soundProvider.Source = new List<ISound>() { soundA, soundB, soundC };
            soundProvider.LoopPlay = true;
            soundProvider.Index = 1;

            Assert.AreEqual(soundProvider.GetSound(), soundB);
            Assert.AreEqual(soundProvider.GetSound(), soundC);
            Assert.AreEqual(soundProvider.GetSound(), soundA);
        }
    }
}