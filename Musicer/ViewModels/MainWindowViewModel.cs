namespace Musicer.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Controls;
    using Musicer.Models.Files;
    using Prism.Commands;
    using Prism.Mvvm;

    public class MainWindowViewModel : BindableBase
    {
        private string title = "Prism Application";

        private ExtendFileInfo selectedDirectory;
        private ObservableCollection<ExtendFileInfo> directories = new ObservableCollection<ExtendFileInfo>();
        private DelegateCommand<TreeView> setTreeViewSelectedItemCommand;

        public MainWindowViewModel()
        {
            Directories.Add(new ExtendFileInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)));
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public ObservableCollection<ExtendFileInfo> Directories { get => directories; set => SetProperty(ref directories, value); }

        public DelegateCommand<TreeView> SetTreeViewSelectedItemCommand
        {
            get => setTreeViewSelectedItemCommand ?? (setTreeViewSelectedItemCommand = new DelegateCommand<TreeView>((tv) =>
            {
                selectedDirectory = tv.SelectedItem as ExtendFileInfo;
            }));
        }
    }
}
