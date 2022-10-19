﻿using System;
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

        public event Action<IDialogResult> RequestClose;

        public string Title => "Listen history";

        public List<ListenHistory> ListenHistories { get => listenHistories; set => SetProperty(ref listenHistories, value); }

        public DelegateCommand CloseCommand => new DelegateCommand(() =>
        {
            RequestClose?.Invoke(new DialogResult());
        });

        public DelegateCommand ReloadCommand => new DelegateCommand(() =>
        {
            int counter = 0;
            var histories = dbContext.GetHistories(200);
            histories.ForEach(l => l.Index = ++counter);
            listenHistories = histories;
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