namespace Musicer.Models.Sounds
{
    using System.Collections.Generic;

    public class SoundProvider
    {
        private List<ISound> source;

        public bool LoopPlay { get; set; }

        public int Index { get; set; }

        public List<ISound> Source
        {
            get => source;
            set
            {
                source = value;
                Index = 0;
            }
        }

        /// <summary>
        /// 実行する毎に Source の中のサウンドを順番に取得します。
        /// </summary>
        /// <returns>Source に含まれるサウンドを取得します。</returns>
        public ISound GetSound()
        {
            if (Source == null || Source.Count == 0)
            {
                return null;
            }

            if (Source.Count < Index + 1)
            {
                if (!LoopPlay)
                {
                    return null;
                }
                else
                {
                    Index = 0;
                }
            }

            return Source[Index++];
        }

        /// <summary>
        /// 実行する毎に Source の中のサウンドを逆順で取得します。
        /// </summary>
        /// <returns>Source に含まれるサウンドを取得します</returns>
        public ISound GetPreviousSound()
        {
            if (Source == null || Source.Count == 0)
            {
                return null;
            }

            Index--;

            if (Index < 0)
            {
                if (!LoopPlay)
                {
                    return null;
                }
                else
                {
                    Index = Source.Count - 1;
                }
            }

            return Source[Index];
        }
    }
}
