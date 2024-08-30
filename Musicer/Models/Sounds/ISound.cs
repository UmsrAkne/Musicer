namespace Musicer.Models.Sounds
{
    using System;

    public interface ISound
    {
        event EventHandler Ended;

        event EventHandler BeforeEnd;

        int Index { get; }

        string Name { get; }

        string FullName { get; }

        int ListenCount { get; set; }

        TimeSpan Duration { get; }

        double CurrentPosition { get; }

        double Volume { get; set; }

        bool IsLongSound { get; }

        bool IsPlaying { get; }

        double FrontCut { get; set; }

        double BackCut { get; set; }

        bool IsSkipped { get; set; }

        void Play();

        void Stop();

        /// <summary>
        /// サウンドファイルをロードします。
        /// 読み込みを行う処理のみを実装し、再生開始は Play() で行います。
        /// </summary>
        void Load();
    }
}