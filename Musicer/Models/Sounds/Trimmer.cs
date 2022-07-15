namespace Musicer.Models.Sounds
{
    /// <summary>
    /// 入力されたサウンドの冒頭、末尾をカットするクラスです。
    /// </summary>
    public class Trimmer
    {
        private int inputCount;
        private bool lastSoundIsLong;

        public double FrontCut { get; set; }

        public double BackCut { get; set; }

        public void Cut(ISound sound, bool nextSoundIsLong)
        {
            inputCount++;

            if (inputCount == 1 && sound.IsLongSound)
            {
                sound.BackCut = BackCut;
                return;
            }

            if (sound.IsLongSound)
            {
                if (lastSoundIsLong)
                {
                    sound.FrontCut = FrontCut;
                }

                if (nextSoundIsLong)
                {
                    sound.BackCut = BackCut;
                }
            }

            lastSoundIsLong = sound.IsLongSound;
        }

        public void Reset()
        {
            inputCount = 0;
            lastSoundIsLong = false;
        }
    }
}
