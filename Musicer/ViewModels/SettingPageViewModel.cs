namespace Musicer.ViewModels
{
    using System;
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Services.Dialogs;

    // ReSharper disable once ClassNeverInstantiated.Global
    public class SettingPageViewModel : BindableBase, IDialogAware
    {
        private string rootDirectoryPath = Properties.Settings.Default.RootDirectoryPath;
        private int crossFadeGoDownSec = Properties.Settings.Default.CrossFadeGoDownSec;
        private int crossFadeGoUpSec = Properties.Settings.Default.CrossFadeGoUpSec;

        private int frontCutSec = Properties.Settings.Default.FrontCutSec;
        private int backCutSec = Properties.Settings.Default.BackCutSec;

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

        public int FrontCutSec
        {
            get => frontCutSec;
            set
            {
                Properties.Settings.Default.FrontCutSec = value;
                SetProperty(ref frontCutSec, value);
            }
        }

        public int BackCutSec
        {
            get => backCutSec;
            set
            {
                Properties.Settings.Default.BackCutSec = value;
                SetProperty(ref backCutSec, value);
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