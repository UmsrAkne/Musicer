namespace Musicer.Models.Sounds
{
    using System;
    using System.Collections.Generic;
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

        public event EventHandler PlayStarted;

        public List<ISound> PlayingSound { get; private set; } = new List<ISound>();

        public SoundProvider SoundProvider { get => soundProvider; set => soundProvider = value; }

        public SoundViewer SoundViewer { get; } = new SoundViewer();

        public Trimmer Trimmer { get; } = new Trimmer();

        public double VolumeUpAmount { get; set; } = 0.01;

        public double VolumeDownAmount { get; set; } = 0.01;

        public double VolumeUpperLimit { get => fader.VolumeUpperLimit; set => fader.VolumeUpperLimit = value; }

        public void Play(int index)
        {
            SoundProvider.Index = index;
            SoundViewer.SetAutoUpdate(true);
            timer.Start();
            ToNext();
        }

        public void Stop()
        {
            timer.Stop();

            PlayingSound.ForEach(s =>
            {
                s.Stop();
                s.Ended -= SoundEndedEventHandler;
                s.BeforeEnd -= SoundBeforeEndEventHandler;
            });

            SoundViewer.Reset();
            SoundViewer.SetAutoUpdate(false);
            Trimmer.Reset();
            SoundProvider.Index = 0;
            fader.Reset();
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

        public void UpdateSetting()
        {
            // ボリュームの変化は １秒あたり５回行われる(インターバルが 200ms)
            VolumeUpAmount = 1.0 / Properties.Settings.Default.CrossFadeGoUpSec / 5;
            VolumeDownAmount = 1.0 / Properties.Settings.Default.CrossFadeGoDownSec / 5;

            Trimmer.FrontCut = Properties.Settings.Default.FrontCutSec;
            Trimmer.BackCut = Properties.Settings.Default.BackCutSec;
        }

        private void ToNext()
        {
            var sound = SoundProvider.GetSound();

            if (sound != null)
            {
                var currentIndex = SoundProvider.Index;
                var nextSound = SoundProvider.GetSound();
                var nextSoundIsLongSound = nextSound != null && nextSound.IsLongSound;
                SoundProvider.Index = currentIndex;

                if (sound.IsLongSound && nextSoundIsLongSound)
                {
                    sound.BeforeEnd += SoundBeforeEndEventHandler;
                }
                else
                {
                    sound.Ended += SoundEndedEventHandler;
                }

                PlayingSound.Add(sound);

                while (PlayingSound.Count >= 3)
                {
                    PlayingSound.RemoveAt(0);
                }

                fader.AddSound(sound);
                SoundViewer.Add(sound);
                Trimmer.Cut(sound, nextSoundIsLongSound);
                sound.Play();
                PlayStarted?.Invoke(this, EventArgs.Empty);
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
