namespace Musicer.Models.Sounds
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Threading;

    public class Player
    {
        private SoundProvider soundProvider = new SoundProvider();
        private Fader fader = new Fader();
        private DispatcherTimer timer = new DispatcherTimer();

        public Player()
        {
            timer.Tick += (object sender, EventArgs e) =>
            {
                ExecuteFader();
            };

            timer.Interval = TimeSpan.FromMilliseconds(200);
        }

        public List<ISound> PlayingSound { get; private set; } = new List<ISound>();

        public SoundProvider SoundProvider { get => soundProvider; set => soundProvider = value; }

        public double VolumeUpAmount { get; set; } = 0.01;

        public double VolumeDownAmount { get; set; } = 0.01;

        public void Play()
        {
            SoundProvider.Index = 0;
            timer.Start();
            ToNext();
        }

        public void Stop()
        {
            timer.Stop();
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

        public void ExecuteFader()
        {
            fader.CrossFade(VolumeDownAmount, VolumeUpAmount);
        }

        private void ToNext()
        {
            var sound = SoundProvider.GetSound();

            if (sound != null)
            {
                sound.Play();

                if (sound.IsLongSound)
                {
                    sound.BeforeEnd += SoundBeforeEndEventHandler;
                }
                else
                {
                    sound.Ended += SoundEndedEventHandler;
                }

                PlayingSound.Add(sound);
                fader.AddSound(sound);
            }
        }

        private void SoundBeforeEndEventHandler(object sender, EventArgs e)
        {
            ToNext();
            PlayingSound.Last().Volume = 0;
            (sender as ISound).BeforeEnd -= SoundBeforeEndEventHandler;
        }

        private void SoundEndedEventHandler(object sender, EventArgs e)
        {
            ToNext();
            (sender as ISound).Ended -= SoundEndedEventHandler;
        }
    }
}
