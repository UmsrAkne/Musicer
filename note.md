# Note

�v���W�F�N�g�Ɋւ��郁�����͂���

## Musicer ���s�̗���

entryPoint -> MainWindow(MainWindowViewModel)

MainWindowViewModel �𐶐�

MainWindowViewModel �̃R���X�g���N�^�����s

�ȉ��̎�v�ȃt�B�[���h��v���p�e�B���Z�b�g�����

MainWindow �̎�v�ȃt�B�[���h�A�v���p�e�B

	private Player player;

	// �f�B���N�g���c���[ �� ItemsSource 
	public ObservableCollection<ExtendFileInfo> Directories {get; set;}

	// �T�E���h�t�@�C�����X�g�� ItemsSource
	public List<ISound> Musics {get; set;}

	// SoundViewr �� player �� MainWindowVM �̗��҂��擾�\�ȏ��
	public SoundViewer SoundViewer => player.SoundViewer;

	public DelegateCommand<TreeView> SetTreeViewSelectedItemCommand
	public DelegateCommand PlayCommand 
	public DelegateCommand StopCommand 
	public DelegateCommand PlayFromIndexCommand

�f�B���N�g���c���[�𑀍삵�A�T�E���h�t�@�C�����܂ރf�B���N�g�����I�����ꂽ���AMainWindow �� `InvokeCommandAction` �ɂ���� SetTreeViewSelectedItemCommand �����s�����B

����ɂ��A`MainWindowViewModel.LoadMusics()` �����s����A`Musics` �Ƀ\�[�X���Z�b�g�B��ʉE���ɃT�E���h�t�@�C���̃��X�g���\�������B

���̏�ԂŁA�Đ��{�^�����������ƁA�Đ��p�̃R�}���h `PlayCommand` �����󋵂ɂ����s����A�T�E���h�t�@�C�����Đ�����B

���̎��A`player` ���� `SoundViewer` �ɃT�E���h�t�@�C���̖��O���Z�b�g����A��ʂɕ\�������

`player` �����ł� `SoundProvider` �ɂ���āA�Đ����� `ISound` �I�u�W�F�N�g���I���A��������A�A���Đ����p������B
