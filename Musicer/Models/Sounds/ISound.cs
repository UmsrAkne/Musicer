namespace Musicer.Models.Sounds
{
    using System;

    public interface ISound
    {
        event EventHandler Ended;

        event EventHandler BeforeEnd;

        string Name { get; }

        TimeSpan Duration { get; }

        double CurrentPosition { get; }

        double Volume { get; set; }

        bool IsLongSound { get; }

        void Play();

        void Stop();
    }
}
