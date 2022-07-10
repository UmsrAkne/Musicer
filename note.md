# Note

プロジェクトに関するメモ等はここ

## Musicer 実行の流れ

entryPoint -> MainWindow(MainWindowViewModel)

MainWindowViewModel を生成

MainWindowViewModel のコンストラクタを実行

以下の主要なフィールドやプロパティがセットされる

MainWindow の主要なフィールド、プロパティ

	private Player player;

	// ディレクトリツリー の ItemsSource 
	public ObservableCollection<ExtendFileInfo> Directories {get; set;}

	// サウンドファイルリストの ItemsSource
	public List<ISound> Musics {get; set;}

	// SoundViewr は player と MainWindowVM の両者が取得可能な状態
	public SoundViewer SoundViewer => player.SoundViewer;

	public DelegateCommand<TreeView> SetTreeViewSelectedItemCommand
	public DelegateCommand PlayCommand 
	public DelegateCommand StopCommand 
	public DelegateCommand PlayFromIndexCommand

ディレクトリツリーを操作し、サウンドファイルを含むディレクトリが選択された時、MainWindow の `InvokeCommandAction` によって SetTreeViewSelectedItemCommand が実行される。

これにより、`MainWindowViewModel.LoadMusics()` が実行され、`Musics` にソースをセット。画面右側にサウンドファイルのリストが表示される。

この状態で、再生ボタン等を押すと、再生用のコマンド `PlayCommand` 等が状況により実行され、サウンドファイルを再生する。

この時、`player` から `SoundViewer` にサウンドファイルの名前がセットされ、画面に表示される

`player` 内部では `SoundProvider` によって、再生する `ISound` オブジェクトが選択、供給され、連続再生を継続する。
