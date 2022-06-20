namespace Musicer.Models.Sounds
{
    using System;
    using System.IO;
    using System.Windows.Threading;
    using NAudio.Wave;

    public class Sound : ISound
    {
        private FileInfo fileInfo;
        private AudioFileReader reader;
        private WaveOutEvent waveOut;
        private DispatcherTimer timer;

        public Sound(FileInfo f)
        {
            fileInfo = f;
        }

        public event EventHandler Ended;

        public event EventHandler BeforeEnd;

        public static TimeSpan LongSoundLength { get; set; } = TimeSpan.FromSeconds(50);

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
                    return false;
                }

                return reader.TotalTime >= LongSoundLength;
            }
        }

        public void Play()
        {
            reader = new AudioFileReader(fileInfo.FullName);
            waveOut = new WaveOutEvent();
            waveOut.Init(reader);
            waveOut.Play();
            waveOut.PlaybackStopped += EndedEventHandler;
            Duration = reader.TotalTime;

            timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(Duration.TotalSeconds - 15) };
            timer.Tick += BeforeEndEventHandler;
            timer.Start();
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
        }

        private void EndedEventHandler(object sender, EventArgs e)
        {
            waveOut.PlaybackStopped -= EndedEventHandler;
            Ended?.Invoke(this, EventArgs.Empty);
        }

        private void BeforeEndEventHandler(object sender, EventArgs e)
        {
            timer.Tick -= BeforeEndEventHandler;
            BeforeEnd?.Invoke(this, EventArgs.Empty);
        }
    }
}
