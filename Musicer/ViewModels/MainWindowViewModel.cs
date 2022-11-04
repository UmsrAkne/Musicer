using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using Musicer.Models.Databases;
using Musicer.Models.Files;
using Musicer.Models.Sounds;
using Musicer.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace Musicer.ViewModels
{
    // ReSharper disable once UnusedType.Global
    public class MainWindowViewModel : BindableBase
    {
        private readonly Player player = new Player();
        private readonly IDialogService dialogService;
        private readonly ListenHistoryDbContext listenHistoryDbContext = new ListenHistoryDbContext();

        private string title = $"Musicer [ {FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion} ]";
        private ExtendFileInfo selectedDirectory;
        private ObservableCollection<ExtendFileInfo> directories;
        private List<ISound> musics;
        private int selectedSoundIndex;
        private double volume = 1.0;
        private bool loopPlay;
        private DelegateCommand<TreeView> setTreeViewSelectedItemCommand;

        private string currentDirectoryPath;

        public MainWindowViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            listenHistoryDbContext.Database.EnsureCreated();
            LoadRootDirectory(Properties.Settings.Default.RootDirectoryPath);

            player.PlayStarted += (sender, e) =>
            {
                SelectedSoundIndex = player.StartedSoundInfo.Index - 1;
                var context = new ListenHistoryDbContext();
                context.Database.EnsureCreated();
                context.AddListenCount(player.StartedSoundInfo);
                RaisePropertyChanged(nameof(PlayingMusicName));
            };

            Volume = Properties.Settings.Default.Volume;
            LoopPlay = true;
            player.UpdateSetting();
        }

        public string Title { get => title; set => SetProperty(ref title, value); }

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
             => player == null || player.PlayingSound.Count == 0 ? string.Empty : player.PlayingSound.Last().Name;

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

        public DelegateCommand ShowHistoryWindowCommand => new DelegateCommand(() =>
        {
            var dialogParam = new DialogParameters { { nameof(ListenHistoryDbContext), listenHistoryDbContext } };
            dialogService.ShowDialog(nameof(HistoryPage), dialogParam, result => { });
        });

        public DelegateCommand RandomSortCommand => new DelegateCommand(() =>
        {
            Musics = Musics.OrderBy(i => Guid.NewGuid()).ToList();
            player.SoundProvider.Source = Musics;
            ReIndex();
            SelectedSoundIndex = 0;
        });

        public DelegateCommand NameSortCommand => new DelegateCommand(() =>
        {
            Musics = Musics.OrderBy(s => s.Name).ToList();
            player.SoundProvider.Source = Musics;
            ReIndex();
            SelectedSoundIndex = 0;
        });

        /// <summary>
        /// ディレクトリ、またはM3U ファイルに含まれているサウンドファイルを読み込んでビューのリストに表示します。
        /// </summary>
        /// <param name="directory">サウンドファイルを含むディレクトリ、または M3U を指す ExtendFileInfo を入力します。</param>
        private void LoadMusics(ExtendFileInfo directory)
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

                // ReSharper disable once UnusedVariable
                Task task = LoadSounds(Musics);
                return;
            }

            if (directory.HasSoundFile)
            {
                Musics = GetSoundFiles(Directory.GetFiles(directory.FileSystemInfo.FullName).ToList());
                player.SoundProvider.Source = Musics;

                // ReSharper disable once UnusedVariable
                Task task = LoadSounds(Musics);
                ReIndex();
            }
        }

        /// <summary>
        /// ルートディレクトリ（このアプリの読み込みの起点となるディレクトリ）を読み込みます。
        /// ルートディレクトリは設定で決めることができますが、設定されていない場合はユーザーの MyMusic が読み込まれます。
        /// また、前回のアプリ終了時に選択していたディレクトリが選択可能な場合、そのフォルダがデフォルトで展開、選択状態になります。
        /// </summary>
        /// <param name="path">ルートディレクトリのパスを入力します。</param>
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

                LoadMusics(dir ?? defaultFileInfo);
            }

            Directories.Add(defaultFileInfo);
        }

        /// <summary>
        /// ファイルパスのリストから ISound のリストを生成します。
        /// </summary>
        /// <param name="soundFilePaths">ファイルパスのリストです。サウンドファイル以外のファイルはスキップされます。</param>
        /// <returns>サウンドファイルの情報を格納した ISound のリストを返します。</returns>
        private List<ISound> GetSoundFiles(List<string> soundFilePaths)
        {
            var sounds = soundFilePaths
            .Select(path => new ExtendFileInfo(path))
            .Where(f => f.IsSoundFile)
            .Select(f => new Sound(f.FileSystemInfo as FileInfo))
            .Cast<ISound>()
            .ToList();

            sounds.ForEach(s => s.ListenCount = listenHistoryDbContext.GetListenCount(s.FullName));
            return sounds;
        }

        /// <summary>
        /// Musics 内の要素に Index を設定します。番号はリストのインデックス順で割り振られます。
        /// </summary>
        private void ReIndex()
        {
            Enumerable.Range(0, Musics.Count).ToList().ForEach(i => ((Sound)Musics[i]).Index = i + 1);
        }

        private async Task LoadSounds(List<ISound> sounds)
        {
          await Task.Run(() => sounds.ForEach(s =>
          {
              var soundInfo = listenHistoryDbContext.GetSoundData(s.FullName);
              if (soundInfo == null || soundInfo.PlaybackTimeTicks == 0)
              {
                  s.Load();
                  listenHistoryDbContext.UpdateSoundDuration(s);
              }
              else
              {
                  ((Sound)s).Duration = new TimeSpan(soundInfo.PlaybackTimeTicks);
              }
          }));
        }
    }
}