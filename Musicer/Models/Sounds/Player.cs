namespace Musicer.Models.Sounds
{
    using System.Collections.Generic;
    using System.Linq;

    public class Player
    {
        private SoundProvider soundProvider = new SoundProvider();

        public List<ISound> PlayingSound { get; private set; } = new List<ISound>();

        public SoundProvider SoundProvider { get => soundProvider; set => soundProvider = value; }

        public void Play()
        {
            SoundProvider.Index = 0;
            ToNext();
        }

        public void Stop()
        {
        }

        public void Pause()
        {
        }

        public void Next()
        {
        }

        public void Prev()
        {
        }

        private void ToNext()
        {
            var sound = SoundProvider.GetSound();

            if (sound != null)
            {
                PlayingSound.Add(sound);
                PlayingSound.LastOrDefault().Play();
            }
        }
    }
}
