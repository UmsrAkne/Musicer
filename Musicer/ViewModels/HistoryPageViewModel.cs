namespace Musicer.ViewModels
{
    using System;
    using System.Collections.Generic;
    using Musicer.Models.Databases;
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Services.Dialogs;

    public class HistoryPageViewModel : BindableBase, IDialogAware
    {
        private List<ListenHistory> listenHistories;
        private ListenHistoryDbContext dbContext;

        public event Action<IDialogResult> RequestClose;

        public string Title => "Listen history";

        public List<ListenHistory> ListenHistories { get => listenHistories; set => SetProperty(ref listenHistories, value); }

        public DelegateCommand CloseCommand => new DelegateCommand(() =>
        {
            RequestClose?.Invoke(new DialogResult());
        });

        public DelegateCommand ReloadCommand => new DelegateCommand(() =>
        {
            listenHistories = dbContext.GetAll();
        });

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            dbContext = parameters.GetValue<ListenHistoryDbContext>(nameof(ListenHistoryDbContext));
            ReloadCommand.Execute();
        }
    }
}
