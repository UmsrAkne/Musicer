using Prism.Mvvm;

namespace Musicer.Models
{
    public class Layout : BindableBase
    {
        private int viewColumnSpan = 2;
        private int viewRowSpan = 1;
        private int soundListColumnIndex;
        private int soundListRowIndex = 2;

        public int ViewColumnSpan
        {
            get => viewColumnSpan;
            private set => SetProperty(ref viewColumnSpan, value);
        }

        public int ViewRowSpan
        {
            get => viewRowSpan;
            private set => SetProperty(ref viewRowSpan, value);
        }

        public int SoundListColumnIndex
        {
            get => soundListColumnIndex;
            private set => SetProperty(ref soundListColumnIndex, value);
        }

        public int SoundListRowIndex
        {
            get => soundListRowIndex;
            private set => SetProperty(ref soundListRowIndex, value);
        }

        public void ToggleLayout()
        {
            if (ViewColumnSpan == 2)
            {
                // 縦分割レイアウト
                ViewColumnSpan = 1;
                ViewRowSpan = 2;
                SoundListColumnIndex = 2;
                SoundListRowIndex = 1;
            }
            else
            {
                // 横分割レイアウト
                ViewColumnSpan = 2;
                ViewRowSpan = 1;
                SoundListColumnIndex = 0;
                SoundListRowIndex = 2;
            }
        }
    }
}