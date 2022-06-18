namespace Musicer.Models.Sounds
{
    using System;
    using System.Collections.Generic;

    public class Player
    {
        private SoundProvider soundProvider = new SoundProvider();
        private Fader fader = new Fader();

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
                if (sound.IsLongSound)
                {
                    sound.BeforeEnd += SoundBeforeEndEventHandler;
                }
                else
                {
                    sound.Ended += SoundEndedEventHandler;
                }

                PlayingSound.Add(sound);
                sound.Play();
                fader.AddSound(sound);
            }
        }

        private void SoundBeforeEndEventHandler(object sender, EventArgs e)
        {
            ToNext();
            (sender as ISound).BeforeEnd -= SoundBeforeEndEventHandler;
        }

        private void SoundEndedEventHandler(object sender, EventArgs e)
        {
            ToNext();
            (sender as ISound).Ended -= SoundEndedEventHandler;
        }
    }
}
