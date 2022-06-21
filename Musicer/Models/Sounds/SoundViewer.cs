namespace Musicer.Models.Sounds
{
    using System.Collections.Generic;
    using Prism.Mvvm;

    public class SoundViewer : BindableBase
    {
        private List<ISound> sounds = new List<ISound>();
        private string playingMusicName;

        public string PlayingMusicName { get => playingMusicName; set => SetProperty(ref playingMusicName, value); }

        public void Add(ISound sound)
        {
            sounds.Add(sound);

            while (sounds.Count > 2)
            {
                sounds.RemoveAt(0);
            }

            if (sounds.Count == 0)
            {
                PlayingMusicName = string.Empty;
            }
            else if (sounds.Count == 1)
            {
                PlayingMusicName = sounds[0].Name;
            }
            else
            {
                PlayingMusicName = $"{sounds[0].Name} >>> {sounds[1].Name}";
            }
        }
    }
}
