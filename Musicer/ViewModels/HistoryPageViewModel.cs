using System;
using System.Collections.Generic;
using Musicer.Models.Databases;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace Musicer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class HistoryPageViewModel : BindableBase, IDialogAware
    {
        private List<ListenHistory> listenHistories;
        private ListenHistoryDbContext dbContext;

        private int pageCount;
        private int maxPageNumber;

        public event Action<IDialogResult> RequestClose;

        public string Title => "Listen history";

        public List<ListenHistory> ListenHistories
        {
            get => listenHistories; private set => SetProperty(ref listenHistories, value);
        }

        public int PageCount { get => pageCount; private set => SetProperty(ref pageCount, value); }

        public int MaxPageNumber { get => maxPageNumber; private set => SetProperty(ref maxPageNumber, value); }

        public DelegateCommand CloseCommand => new DelegateCommand(() =>
        {
            RequestClose?.Invoke(new DialogResult());
        });

        public DelegateCommand ReloadCommand => new DelegateCommand(() =>
        {
            var displayCount = Properties.Settings.Default.HistoryDisplayCount;
            ListenHistories = dbContext.GetHistories(PageCount * displayCount, displayCount);
            MaxPageNumber = (int)Math.Floor((double)dbContext.GetHistoryCount() / displayCount);
        });

        public DelegateCommand NextPageCommand => new DelegateCommand(() =>
        {
            if (PageCount >= MaxPageNumber)
            {
                return;
            }

            PageCount++;
            ReloadCommand.Execute();
        });

        public DelegateCommand PrevPageCommand => new DelegateCommand(() =>
        {
            if (PageCount <= 0)
            {
                return;
            }

            PageCount--;
            ReloadCommand.Execute();
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
            PageCount = 0;
            dbContext = parameters.GetValue<ListenHistoryDbContext>(nameof(ListenHistoryDbContext));
            ReloadCommand.Execute();
        }
    }
}