using Braco.Services;
using Braco.Utilities;
using Braco.Utilities.Wpf;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Input;

namespace AudioDownloader.WpfClient
{
	[Page(AllowGoingToPreviousPage = true)]
	public class SettingsPageViewModel : PageViewModel
	{
		private readonly IChooserDialogsService _chooserDialogsService;

		#region Language

		public List<CultureInfo> Languages { get; set; }

		private CultureInfo _language;
		public CultureInfo Language
		{
			get => _language;
			set
			{
				if (Equals(value, _language)) return;

				if (_language == null)
				{
					_language = value;
					return;
				}

				while (!_localizer.ChangeLanguage(value.Name)) ;

				_language = value;
				_config.Set(ConfigurationKeys.Language, value.Name);
			}
		}

		#endregion

		#region Video Download Directory

		private string _videoDownloadDirectory;

		[Setting]
		public string VideoDownloadDirectory
		{
			get => _videoDownloadDirectory;
			set
			{
				if (Equals(value, _videoDownloadDirectory)) return;

				_videoDownloadDirectory = PathUtilities.GetPathWithoutInvalidChars(value);
			}
		}

		#endregion

		[Setting]
		public bool DeleteVideoAfterMP3Conversion { get; set; }

		#region Audio Download Directory

		private string _audioDownloadDirectory;

		[Setting]
		public string AudioDownloadDirectory
		{
			get => _audioDownloadDirectory;
			set
			{
				if (Equals(value, _audioDownloadDirectory)) return;

				_audioDownloadDirectory = PathUtilities.GetPathWithoutInvalidChars(value);
			}
		} 

		#endregion

		[Setting]
		public bool DeleteAudioSplitSourceFile { get; set; }

		public ICommand PickVideoDirectoryCommand { get; set; }
		public ICommand PickAudioDirectoryCommand { get; set; } 

		public SettingsPageViewModel(IChooserDialogsService chooserDialogsService)
		{
			_chooserDialogsService = chooserDialogsService ?? throw new ArgumentNullException(nameof(chooserDialogsService));
			
			PickVideoDirectoryCommand = new RelayCommand(OnPickVideoDirectory);
			PickAudioDirectoryCommand = new RelayCommand(OnPickAudioDirectory);

			Languages = _readOnlyConfig
				.GetSection(Initializer.InitializerSectionName)
				.GetSection(ISetupService.ServicesSetupSection)
				.GetSection(WpfDictionaryLocalizerSetup.SectionName)
				.GetSection(WpfDictionaryLocalizerSetup.CulturesSectionName)
				.Get<string[]>()
				.Select(culture => new CultureInfo(culture))
				.ToList();

			Language = new CultureInfo(_localizer.Culture);
		}

		private void OnPickVideoDirectory()
		{
			var directory = _chooserDialogsService.ChooseDirectory(_localizer[LocalizationKeys.Settings_PickVideoDownloadDirectoryTitle], _config[ConfigurationKeys.VideoDownloadDirectory]);

			if (directory == null) return;

			VideoDownloadDirectory = directory;
		}

		private void OnPickAudioDirectory()
		{
			var directory = _chooserDialogsService.ChooseDirectory(_localizer[LocalizationKeys.Settings_PickAudioDownloadDirectoryTitle], _config[ConfigurationKeys.AudioDownloadDirectory]);

			if (directory == null) return;

			AudioDownloadDirectory = directory;
		}
	}
}
