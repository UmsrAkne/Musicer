namespace Musicer.Models.Sounds
{
    using System;

    public interface ISound
    {
        event EventHandler Ended;

        event EventHandler BeforeEnd;

        string Name { get; }

        string FullName { get; }

        int ListenCount { get; set; }

        TimeSpan Duration { get; }

        double CurrentPosition { get; }

        double Volume { get; set; }

        bool IsLongSound { get; }

        bool IsPlaying { get; }

        double FrontCut { get; set; }

        double BackCut { get; set; }

        void Play();

        void Stop();
    }
}
