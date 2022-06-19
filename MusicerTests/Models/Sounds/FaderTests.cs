namespace Musicer.Models.Sounds.Tests
{
    using NUnit.Framework;

    public class FaderTests
    {
        [Test]
        public void AddSoundTest()
        {
            Fader fader = new Fader();
            Assert.AreEqual(fader.ManagementSoundCount, 0);

            fader.AddSound(new DummySound());
            Assert.AreEqual(fader.ManagementSoundCount, 1);

            fader.AddSound(new DummySound());
            Assert.AreEqual(fader.ManagementSoundCount, 2);

            fader.AddSound(new DummySound());
            Assert.AreEqual(fader.ManagementSoundCount, 2);

            fader.AddSound(new DummySound());
            Assert.AreEqual(fader.ManagementSoundCount, 2);
        }

        [Test]
        [Description("AddSound()のテスト")]
        public void 同じサウンドを入力した際の挙動のテスト()
        {
            Fader fader = new Fader();
            Assert.AreEqual(fader.ManagementSoundCount, 0);

            var dummyA = new DummySound();
            var dummyB = new DummySound();

            fader.AddSound(dummyA);
            Assert.AreEqual(fader.ManagementSoundCount, 1);

            fader.AddSound(dummyA);
            Assert.AreEqual(fader.ManagementSoundCount, 1, "連続で同じものを入れても弾かれるので要素数は維持される。");

            fader.AddSound(dummyB);
            Assert.AreEqual(fader.ManagementSoundCount, 2, "別の要素で、要素数 1 であるので要素数が増える");

            fader.AddSound(dummyB);
            Assert.AreEqual(fader.ManagementSoundCount, 2, "数は増えない");
        }

        [Test]
        public void サウンド入力０と１のときのクロスフェードのテスト()
        {
            Fader fader = new Fader();
            fader.VolumeUpperLimit = 1.0;

            var sound1 = new DummySound();

            // 入力なしで実行してもエラーにならないか確認する。
            fader.CrossFade(0.1, 0.1);

            Assert.AreEqual(sound1.Volume, 1.0);

            fader.AddSound(sound1);

            fader.CrossFade(0.1, 0.1);
            Assert.AreEqual(sound1.Volume, 1.0, "サウンドを一つの段階では CrossFade() は動作しないので値は維持");

            fader.CrossFade(0.1, 0.1);
            Assert.AreEqual(sound1.Volume, 1.0, "複数回実行しても同じく値は維持されているか");
        }

        [Test]
        public void サウンドを２つ入力している状態のテスト()
        {
            Fader fader = new Fader();
            fader.VolumeUpperLimit = 1.0;

            var sound1 = new DummySound();
            var sound2 = new DummySound();
            sound1.Volume = 1;
            sound2.Volume = 0;

            fader.AddSound(sound1);
            fader.AddSound(sound2);

            fader.CrossFade(0.3, 0.4);
            Assert.AreEqual(sound1.Volume, 0.7, 0.001);
            Assert.AreEqual(sound2.Volume, 0.4, 0.001);

            fader.CrossFade(0.3, 0.4);
            Assert.AreEqual(sound1.Volume, 0.4, 0.001);
            Assert.AreEqual(sound2.Volume, 0.8, 0.001);

            fader.CrossFade(0.3, 0.4);
            Assert.AreEqual(sound1.Volume, 0.1, 0.001);
            Assert.AreEqual(sound2.Volume, 1.0, 0.001);

            fader.CrossFade(0.3, 0.4);
            Assert.AreEqual(sound1.Volume, 0, 0.001);
            Assert.AreEqual(sound2.Volume, 1.0, 0.001);

            fader.CrossFade(0.3, 0.4);
            Assert.AreEqual(sound1.Volume, 0, 0.001);
            Assert.AreEqual(sound2.Volume, 1.0, 0.001);
        }
    }
}