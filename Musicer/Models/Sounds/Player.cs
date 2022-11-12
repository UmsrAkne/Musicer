namespace Musicer.Models.Sounds
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Threading;

    public class Player
    {
        private readonly Fader fader = new Fader();
        private readonly DispatcherTimer timer = new DispatcherTimer();

        public Player()
        {
            timer.Tick += (sender, e) =>
            {
                ExecuteFader();
            };

            timer.Interval = TimeSpan.FromMilliseconds(200);
        }

        public event EventHandler PlayStarted;

        public List<ISound> PlayingSound { get; private set; } = new List<ISound>();

        public SoundProvider SoundProvider { get; set; } = new SoundProvider();

        public SoundViewer SoundViewer { get; } = new SoundViewer();

        /// <summary>
        /// 最後に再生開始したサウンドの情報が格納されています。
        /// 型はダミーなので、このプロパティを介してプレイヤーのサウンドを操作することは一切できません。
        /// サウンドの情報を取得する用途でのみ使用します。
        /// </summary>
        /// <value>最後に再生開始したサウンドの情報を記録したダミーサウンド</value>
        public DummySound StartedSoundInfo { get; private set; }

        public double VolumeUpperLimit { get => fader.VolumeUpperLimit; set => fader.VolumeUpperLimit = value; }

        private double VolumeUpAmount { get; set; } = 0.01;

        private double VolumeDownAmount { get; set; } = 0.01;

        private Trimmer Trimmer { get; } = new Trimmer();

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
                s.Volume = 1.0;
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
                sound.ListenCount++;
                sound.Play();

                StartedSoundInfo = new DummySound()
                {
                    FullName = sound.FullName,
                    Name = sound.Name,
                    Index = sound.Index,
                };

                PlayStarted?.Invoke(this, EventArgs.Empty);
            }
        }

        private void SoundBeforeEndEventHandler(object sender, EventArgs e)
        {
            ToNext();
            ((ISound)sender).BeforeEnd -= SoundBeforeEndEventHandler;
        }

        private void SoundEndedEventHandler(object sender, EventArgs e)
        {
            ToNext();
            ((ISound)sender).Ended -= SoundEndedEventHandler;
        }

        private void ExecuteFader()
        {
            fader.CrossFade(VolumeDownAmount, VolumeUpAmount);
        }
    }
}