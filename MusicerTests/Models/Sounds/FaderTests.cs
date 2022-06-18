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
    }
}