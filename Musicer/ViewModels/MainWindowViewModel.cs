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
        private ObservableCollection<ExtendFileInfo> directories;
        private List<ISound> musics;
        private int selectedSoundIndex;
        private double volume = 1.0;
        private bool loopPlay;
        private DelegateCommand<TreeView> setTreeViewSelectedItemCommand;
        private IDialogService dialogService;

        private string currentDirectoryPath;

        public MainWindowViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            LoadRootDirectory(Properties.Settings.Default.RootDirectoryPath);

            player.PlayStarted += (sedenr, e) => RaisePropertyChanged(nameof(PlayingMusicName));
            Volume = Properties.Settings.Default.Volume;
            LoopPlay = true;
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

        public bool LoopPlay
        {
            get => loopPlay;
            set
            {
                SetProperty(ref loopPlay, value);
                player.SoundProvider.LoopPlay = value;
            }
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
            var currentRootPath = Properties.Settings.Default.RootDirectoryPath;

            dialogService.ShowDialog(nameof(SettingPage), new DialogParameters(), result => { });

            if (currentRootPath != Properties.Settings.Default.RootDirectoryPath)
            {
                LoadRootDirectory(Properties.Settings.Default.RootDirectoryPath);
            }

            player.UpdateSetting();
        });

        public void LoadMusics(ExtendFileInfo directory)
        {
            if (directory == null)
            {
                Musics = new List<ISound>();
                return;
            }

            selectedDirectory = directory;
            CurrentDirectoryPath = directory.FileSystemInfo.FullName;
            Properties.Settings.Default.LastSelectedDirectoryPath = directory.FileSystemInfo.FullName;
            Properties.Settings.Default.Save();

            if (directory.IsM3U)
            {
                var fileInfos = directory.GetPlayListFromM3U(File.ReadAllLines(directory.FileSystemInfo.FullName));
                Musics = GetSoundFiles(fileInfos.Select(f => f.FileSystemInfo.FullName).ToList());
                player.SoundProvider.Source = Musics;
                ReIndex();
                return;
            }

            if (directory.HasSoundFile)
            {
                Musics = GetSoundFiles(Directory.GetFiles(directory.FileSystemInfo.FullName).ToList());
                player.SoundProvider.Source = Musics;
                ReIndex();
            }
        }

        private void LoadRootDirectory(string path)
        {
            Directories = new ObservableCollection<ExtendFileInfo>();

            ExtendFileInfo defaultFileInfo = (!string.IsNullOrWhiteSpace(path) && Directory.Exists(path))
                ? new ExtendFileInfo(path)
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

        private void ReIndex()
        {
            Enumerable.Range(0, Musics.Count).ToList().ForEach(i => (Musics[i] as Sound).Index = i + 1);
        }
    }
}
