namespace Musicer.Models.Sounds
{
    using System;
    using System.IO;
    using System.Windows.Threading;
    using NAudio.Wave;
    using Prism.Mvvm;

    public class Sound : BindableBase, ISound
    {
        private FileInfo fileInfo;
        private AudioFileReader reader;
        private WaveOutEvent waveOut;
        private DispatcherTimer timer;

        private int index;

        private bool isPlaying;

        public Sound(FileInfo f)
        {
            fileInfo = f;
        }

        public event EventHandler Ended;

        public event EventHandler BeforeEnd;

        public static TimeSpan LongSoundLength
        {
            get
            {
                var fadeDuration = Properties.Settings.Default.CrossFadeGoDownSec + Properties.Settings.Default.CrossFadeGoUpSec;
                return TimeSpan.FromSeconds(fadeDuration * 1.2);
            }
        }

        public string Name => fileInfo.Name;

        public TimeSpan Duration { get; set; }

        public double CurrentPosition => reader != null ? reader.CurrentTime.TotalSeconds : 0;

        public double Volume
        {
            get => reader != null ? reader.Volume : 0;
            set
            {
                if (reader != null)
                {
                    reader.Volume = (float)value;
                }
            }
        }

        public bool IsLongSound
        {
            get
            {
                if (reader == null)
                {
                    reader = new AudioFileReader(fileInfo.FullName);
                }

                return reader.TotalTime >= TimeSpan.FromSeconds((LongSoundLength.TotalSeconds + FrontCut + BackCut) * 1.5);
            }
        }

        public bool IsPlaying { get => isPlaying; set => SetProperty(ref isPlaying, value); }

        public double FrontCut { get; set; }

        public double BackCut { get; set; }

        public int Index { get => index; set => SetProperty(ref index, value); }

        public void Play()
        {
            if (reader == null)
            {
                reader = new AudioFileReader(fileInfo.FullName);
            }

            reader.CurrentTime = TimeSpan.FromSeconds(FrontCut);

            waveOut = new WaveOutEvent();
            waveOut.Init(reader);
            waveOut.Play();
            waveOut.PlaybackStopped += EndedEventHandler;
            Duration = reader.TotalTime;
            IsPlaying = true;

            if (IsLongSound)
            {
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(Duration.TotalSeconds - FrontCut - BackCut - Properties.Settings.Default.CrossFadeGoDownSec);
                timer.Tick += BeforeEndEventHandler;
                timer.Start();
            }
        }

        public void Stop()
        {
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.PlaybackStopped -= EndedEventHandler;
            }

            if (timer != null)
            {
                timer.Tick -= BeforeEndEventHandler;
                timer.Stop();
                timer = null;
            }

            IsPlaying = false;
        }

        private void EndedEventHandler(object sender, EventArgs e)
        {
            waveOut.PlaybackStopped -= EndedEventHandler;
            IsPlaying = false;
            Ended?.Invoke(this, EventArgs.Empty);
        }

        private void BeforeEndEventHandler(object sender, EventArgs e)
        {
            timer.Tick -= BeforeEndEventHandler;
            BeforeEnd?.Invoke(this, EventArgs.Empty);
        }
    }
}
