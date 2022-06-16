namespace Musicer.Models.Sounds
{
    using System;

    public interface ISound
    {
        string Name { get; }

        TimeSpan Duration { get; }

        double CurrentPosition { get; }

        void Play();

        void Stop();
    }
}
