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
        private string title = "Musicer";

        private Player player = new Player();
        private ExtendFileInfo selectedDirectory;
        private ObservableCollection<ExtendFileInfo> directories = new ObservableCollection<ExtendFileInfo>();
        private List<ISound> musics;
        private int selectedSoundIndex;
        private double volume = 1.0;
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
                var dir = DirectoryExpander.ExpandDirectories(defaultFileInfo, new ExtendFileInfo(Properties.Settings.Default.LastSelectedDirectoryPath));

                if (dir == null)
                {
                    LoadMusics(defaultFileInfo);
                }
                else
                {
                    LoadMusics(dir);
                }
            }

            Directories.Add(defaultFileInfo);
            player.PlayStarted += (sedenr, e) => RaisePropertyChanged(nameof(PlayingMusicName));
            Volume = Properties.Settings.Default.Volume;
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

        public int SelectedSoundIndex { get => selectedSoundIndex; set => SetProperty(ref selectedSoundIndex, value); }

        public double Volume
        {
            get => volume;
            set
            {
                player.VolumeUpperLimit = value;
                SetProperty(ref volume, value);
                Properties.Settings.Default.Volume = value;
                Properties.Settings.Default.Save();
                player.UpdateSetting();
            }
        }

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
            player.Play(0);
        });

        public DelegateCommand StopCommand => new DelegateCommand(() =>
        {
            player.Stop();
        });

        public DelegateCommand PlayFromIndexCommand => new DelegateCommand(() =>
        {
            player.Stop();
            player.Play(SelectedSoundIndex);
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

            if (directory.IsM3U)
            {
                var fileInfos = directory.GetPlayListFromM3U(File.ReadAllLines(directory.FileSystemInfo.FullName));
                Musics = GetSoundFiles(fileInfos.Select(f => f.FileSystemInfo.FullName).ToList());
                player.SoundProvider.Source = Musics;
                return;
            }

            if (directory.HasSoundFile)
            {
                Musics = GetSoundFiles(Directory.GetFiles(directory.FileSystemInfo.FullName).ToList());
                player.SoundProvider.Source = Musics;
            }
        }

        private List<ISound> GetSoundFiles(List<string> soundFilePaths)
        {
            return soundFilePaths
            .Select(path => new ExtendFileInfo(path))
            .Where(f => f.IsSoundFile)
            .Select(f => new Sound(f.FileSystemInfo as FileInfo))
            .Cast<ISound>()
            .ToList();
        }
    }
}
