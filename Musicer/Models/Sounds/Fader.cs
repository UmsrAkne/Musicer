namespace Musicer.Models.Sounds
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 複数の ISound を受け取り、内部で音量の制御を行うクラスです。
    /// </summary>
    public class Fader
    {
        private List<ISound> sounds = new List<ISound>();

        public int ManagementSoundCount => sounds.Count;

        public void AddSound(ISound sound)
        {
            sounds.Add(sound);

            while (sounds.Count > 2)
            {
                var s = sounds.First();
                s.Volume = 0;
                sounds.Remove(s);
            }
        }
    }
}
