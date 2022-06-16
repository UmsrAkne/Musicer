namespace Musicer.Models.Sounds
{
    using System;
    using System.IO;

    public class Sound : ISound
    {
        private FileInfo fileInfo;

        public Sound(FileInfo f)
        {
            fileInfo = f;
        }

        public string Name => throw new NotImplementedException();

        public TimeSpan Duration => throw new NotImplementedException();

        public double CurrentPosition => throw new NotImplementedException();

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
