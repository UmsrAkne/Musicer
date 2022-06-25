namespace Musicer.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Windows.Controls;
    using Musicer.Models.Files;
    using Musicer.Models.Sounds;
    using Musicer.Views;
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Services.Dialogs;

    public class MainWindowViewModel : BindableBase
    {
        private string title = "Prism Application";

        private Player player = new Player();
        private ExtendFileInfo selectedDirectory;
        private ObservableCollection<ExtendFileInfo> directories = new ObservableCollection<ExtendFileInfo>();
        private List<ISound> musics;
        private DelegateCommand<TreeView> setTreeViewSelectedItemCommand;
        private IDialogService dialogService;

        private string currentDirectoryPath;

        public MainWindowViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;

            var rootPath = Properties.Settings.Default.RootDirectoryPath;

            ExtendFileInfo defaultFileInfo = (!string.IsNullOrWhiteSpace(rootPath) && Directory.Exists(rootPath))
                ? new ExtendFileInfo(rootPath)
                : new ExtendFileInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));

            defaultFileInfo.IsExpanded = true;

            if (Properties.Settings.Default.LastSelectedDirectoryPath != string.Empty)
            {
                var dir = DirectoryExpander.ExpandDirectories(defaultFileInfo, new DirectoryInfo(Properties.Settings.Default.LastSelectedDirectoryPath));
                LoadMusics(dir);
            }

            Directories.Add(defaultFileInfo);
            player.PlayStarted += (sedenr, e) => RaisePropertyChanged(nameof(PlayingMusicName));
            player.UpdateSetting();
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public ObservableCollection<ExtendFileInfo> Directories { get => directories; set => SetProperty(ref directories, value); }

        public List<ISound> Musics { get => musics; set => SetProperty(ref musics, value); }

        public SoundViewer SoundViewer => player.SoundViewer;

        public DelegateCommand<TreeView> SetTreeViewSelectedItemCommand
        {
            get => setTreeViewSelectedItemCommand ?? (setTreeViewSelectedItemCommand = new DelegateCommand<TreeView>((tv) =>
            {
                var directory = tv.SelectedItem as ExtendFileInfo;

                if (directory != selectedDirectory)
                {
                    LoadMusics(directory);
                }
            }));
        }

        public string CurrentDirectoryPath { get => currentDirectoryPath; set => SetProperty(ref currentDirectoryPath, value); }

        public string PlayingMusicName
        {
            get => player == null || player.PlayingSound.Count == 0 ? string.Empty : player.PlayingSound.Last().Name;
        }

        public DelegateCommand PlayCommand => new DelegateCommand(() =>
        {
            player.Play();
        });

        public DelegateCommand StopCommand => new DelegateCommand(() =>
        {
            player.Stop();
        });

        public DelegateCommand ShowSettingWindowCommand => new DelegateCommand(() =>
        {
            dialogService.ShowDialog(nameof(SettingPage), new DialogParameters(), result => { });
            player.UpdateSetting();
        });

        public void LoadMusics(ExtendFileInfo directory)
        {
            selectedDirectory = directory;
            CurrentDirectoryPath = directory.FileSystemInfo.FullName;
            Properties.Settings.Default.LastSelectedDirectoryPath = directory.FileSystemInfo.FullName;
            Properties.Settings.Default.Save();

            if (directory.HasSoundFile)
            {
                Musics = (directory.FileSystemInfo as DirectoryInfo).GetFiles().Select(f => new Sound(f) as ISound).ToList();
                player.SoundProvider.Source = Musics;
            }
        }
    }
}
