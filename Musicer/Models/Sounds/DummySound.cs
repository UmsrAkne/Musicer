namespace Musicer.Models.Sounds
{
    using System;

    public class DummySound : ISound
    {
        public event EventHandler Ended;

        public event EventHandler BeforeEnd;

        public string Name { get; set; } = string.Empty;

        public TimeSpan Duration { get; set; }

        public double CurrentPosition { get; set; }

        public bool IsPlaying { get; private set; }

        public void Play()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
