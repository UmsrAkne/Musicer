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

        public string Name => fileInfo.Name;

        public TimeSpan Duration => new TimeSpan(0);

        public double CurrentPosition => 0;

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
