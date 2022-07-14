namespace Musicer.Models.Sounds
{
    using System.Collections.Generic;

    /// <summary>
    /// 入力されたサウンドの冒頭、末尾をカットするクラスです。
    /// </summary>
    public class Trimmer
    {
        public List<ISound> Sounds { get; set; }

        public void Add(ISound sound)
        {
            Sounds.Add(sound);

            while (Sounds.Count > 2)
            {
                Sounds.RemoveAt(0);
            }
        }
    }
}
