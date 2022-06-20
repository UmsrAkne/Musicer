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
        private ISound lastAddedSound;

        public int ManagementSoundCount => sounds.Count;

        public double VolumeUpperLimit { get; set; } = 1.0;

        /// <summary>
        /// AddSound(ISound) によって入力したオブジェクトが２つの時、サウンドのクロスフェードを行います。
        /// また、入力されているサウンドオブジェクトが２つ未満の場合は、このメソッドは何の処理も行いません。
        /// </summary>
        /// <param name="down">古い方のサウンドオブジェクトの音量を指定値だけ下げます</param>
        /// <param name="up">新しい方のサウンドオブジェクトの音量を指定値だけ上げます</param>
        public void CrossFade(double down, double up)
        {
            if (sounds.Count <= 1)
            {
                // sounds の要素が 0 or 1 の時はクロスフェードの必要がないため中断する。
                return;
            }

            var first = sounds.First();
            var last = sounds.Last();

            if (first.Volume >= 0)
            {
                first.Volume -= down;

                if (first.Volume <= 0)
                {
                    first.Volume = 0;
                }
            }

            if (last.Volume <= VolumeUpperLimit)
            {
                last.Volume += up;

                if (last.Volume >= VolumeUpperLimit)
                {
                    last.Volume = VolumeUpperLimit;
                }
            }
        }

        public void AddSound(ISound sound)
        {
            if (lastAddedSound == sound)
            {
                // ２回連続して同じサウンドが入るとこのクラスは動作しないので弾く。
                return;
            }

            sounds.Add(sound);
            lastAddedSound = sound;

            while (sounds.Count > 2)
            {
                var s = sounds.First();
                s.Volume = 0;
                sounds.Remove(s);
            }
        }

        public void Reset()
        {
            sounds = new List<ISound>();
        }
    }
}
