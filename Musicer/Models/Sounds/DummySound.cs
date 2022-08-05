namespace Musicer.Models.Sounds
{
    using System;

    public class DummySound : ISound
    {
        public event EventHandler Ended;

        public event EventHandler BeforeEnd;

        public string Name { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public TimeSpan Duration { get; set; }

        public double CurrentPosition { get; set; }

        public bool IsPlaying { get; private set; }

        public double Volume { get; set; } = 1.0;

        public bool IsLongSound { get; set; }

        public double FrontCut { get; set; }

        public double BackCut { get; set; }

        public int ListenCount { get; set; }

        public int Index { get; set; }

        public void Play()
        {
            IsPlaying = true;
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void DispatchEndedEvent()
        {
            Ended.Invoke(this, EventArgs.Empty);
        }

        public void DispatchBeforeEnd()
        {
            BeforeEnd.Invoke(this, EventArgs.Empty);
        }

        public void Load()
        {
            throw new NotImplementedException();
        }
    }
}
