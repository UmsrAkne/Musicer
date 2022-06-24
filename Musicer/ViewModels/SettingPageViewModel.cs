namespace Musicer.ViewModels
{
    using System;
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Services.Dialogs;

    public class SettingPageViewModel : BindableBase, IDialogAware
    {
        private string rootDirectoryPath = Properties.Settings.Default.RootDirectoryPath;
        private int crossFadeGoDownSec = Properties.Settings.Default.CrossFadeGoDownSec;
        private int crossFadeGoUpSec = Properties.Settings.Default.CrossFadeGoUpSec;

        public event Action<IDialogResult> RequestClose;

        public string Title => "Setting";

        public string RootDirectoryPath
        {
            get => rootDirectoryPath;
            set
            {
                Properties.Settings.Default.RootDirectoryPath = value;
                SetProperty(ref rootDirectoryPath, value);
            }
        }

        public int CrossFadeGoUpSec
        {
            get => crossFadeGoUpSec;
            set
            {
                Properties.Settings.Default.CrossFadeGoUpSec = value;
                SetProperty(ref crossFadeGoUpSec, value);
            }
        }

        public int CrossFadeGoDownSec
        {
            get => crossFadeGoDownSec;
            set
            {
                Properties.Settings.Default.CrossFadeGoDownSec = value;
                SetProperty(ref crossFadeGoDownSec, value);
            }
        }

        public DelegateCommand CloseCommand => new DelegateCommand(() =>
        {
            RequestClose?.Invoke(new DialogResult());
        });

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            Properties.Settings.Default.Save();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}
