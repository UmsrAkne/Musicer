using Prism.Mvvm;

namespace Musicer.Models
{
    public class Layout : BindableBase
    {
        private int treeViewColumnSpan = 1;
        private int soundListColumnIndex = 1;
        private int soundListRowIndex = 2;

        public int TreeViewColumnSpan
        {
            get => treeViewColumnSpan;
            set => SetProperty(ref treeViewColumnSpan, value);
        }

        public int SoundListColumnIndex
        {
            get => soundListColumnIndex;
            set => SetProperty(ref soundListColumnIndex, value);
        }

        public int SoundListRowIndex
        {
            get => soundListRowIndex;
            set => SetProperty(ref soundListRowIndex, value);
        }
    }
}