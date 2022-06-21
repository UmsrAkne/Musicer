namespace Musicer.Models.Sounds
{
    using System.Collections.Generic;
    using Prism.Mvvm;

    public class SoundViewer : BindableBase
    {
        private List<ISound> sounds = new List<ISound>();

        public void Add(ISound sound)
        {
        }
    }
}
