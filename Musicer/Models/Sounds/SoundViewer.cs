namespace Musicer.Models.Sounds
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Threading;
    using Prism.Mvvm;

    public class SoundViewer : BindableBase
    {
        private List<ISound> sounds = new List<ISound>();
        private DispatcherTimer timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(500) };
        private string playingMusicName;
        private TimeSpan currentTime;
        private TimeSpan totalTime;

        public SoundViewer()
        {
            timer.Tick += (sender, e) => Update();
        }

        public string PlayingMusicName { get => playingMusicName; set => SetProperty(ref playingMusicName, value); }

        public TimeSpan CurrentTime { get => currentTime; set => SetProperty(ref currentTime, value); }

        public TimeSpan TotalTime { get => totalTime; set => SetProperty(ref totalTime, value); }

        public void Add(ISound sound)
        {
            sounds.Add(sound);

            while (sounds.Count > 2)
            {
                sounds[0].Ended -= SoundEndedEventHandler;
                sounds.RemoveAt(0);
            }

            sound.Ended += SoundEndedEventHandler;
            Update();
        }

        public void Reset()
        {
            sounds = new List<ISound>();
            Update();
        }

        public void SetAutoUpdate(bool b)
        {
            if (b)
            {
                timer.Start();
            }
            else
            {
                timer.Stop();
            }
        }

        private void Update()
        {
            if (sounds.Count == 0)
            {
                PlayingMusicName = string.Empty;
                CurrentTime = TimeSpan.Zero;
            }
            else if (sounds.Count == 1)
            {
                PlayingMusicName = sounds[0].Name;
                CurrentTime = TimeSpan.FromSeconds(sounds[0].CurrentPosition);
                TotalTime = TimeSpan.FromSeconds(sounds[0].Duration.TotalSeconds);
            }
            else
            {
                PlayingMusicName = $"{sounds[0].Name} >>> {sounds[1].Name}";
                CurrentTime = TimeSpan.FromSeconds(sounds[0].CurrentPosition);
                TotalTime = TimeSpan.FromSeconds(sounds[0].Duration.TotalSeconds);
            }
        }

        private void SoundEndedEventHandler(object sender, System.EventArgs e)
        {
            sounds.Remove(sender as ISound);
            (sender as ISound).Ended -= SoundEndedEventHandler;
            Update();
        }
    }
}
