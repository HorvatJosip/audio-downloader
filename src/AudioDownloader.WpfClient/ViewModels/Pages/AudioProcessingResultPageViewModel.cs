using AutoMapper;
using Braco.Services;
using Braco.Services.Abstractions;
using Braco.Services.Media.Abstractions;
using Braco.Utilities;
using Braco.Utilities.Wpf;
using System.IO;
using System.Windows.Input;

namespace AudioDownloader.WpfClient
{
	[AutoMap(sourceType: typeof(MediaSplitResponse))]
	public class AudioProcessingResultPageViewModel : PageViewModel
	{
		private readonly IProcessStarter _processStarter;

		public FileInfo SplitFile { get; set; }

		public string SongFileName { get; set; }

		public ICommand DoAnotherCommand { get; }
		public ICommand OpenFileCommand { get; }
		public ICommand OpenContainingDirectoryCommand { get; }

		public AudioProcessingResultPageViewModel(IProcessStarter processStarter)
		{
			_processStarter = processStarter;

			DoAnotherCommand = new RelayCommand(OnDoAnother);
			OpenFileCommand = new RelayCommand(OnOpenFile);
			OpenContainingDirectoryCommand = new RelayCommand(OnOpenContainingDirectory);
		}

		private void OnDoAnother()
		{
			_windowService.ChangePage<SourceDefinitionPageViewModel>();
		}

		private void OnOpenFile()
		{
			_processStarter.OpenFile(SplitFile);
		}

		private void OnOpenContainingDirectory()
		{
			_processStarter.ExecuteCommand($"{WindowsProcessStarter.ExplorerFileName} /select, \"{SplitFile.FullName}\"");
		}

		public override void OnLoaded(WindowViewModel windowVM, object pageData, PageViewModel previousPage)
		{
			if (!(previousPage is SettingsPageViewModel))
			{
				base.OnLoaded(windowVM, pageData, previousPage);

				SongFileName = Path.GetFileNameWithoutExtension(SplitFile.FullName);
			}
		}
	}
}
