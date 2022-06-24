namespace Musicer.ViewModels
{
    using System;
    using Prism.Services.Dialogs;

    public class SettingPageViewModel : IDialogAware
    {
        public event Action<IDialogResult> RequestClose;

        public string Title => "Setting";

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}
