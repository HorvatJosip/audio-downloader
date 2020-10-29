using Braco.Services.Media;
using Braco.Utilities;
using Braco.Utilities.Extensions;
using Braco.Utilities.Wpf;
using System;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace AudioDownloader.WpfClient
{
	[Page(AllowGoingToPreviousPage = true)]
	public class MP3UtilitiesPageViewModel : PageViewModel
	{
		private readonly IChooserDialogsService _dialogsService;

		public string AlbumName { get; set; }
		public ICommand ChooseAlbumDirectoryCommand { get; set; }
		public ICommand ChooseAuthorAndTitleDirectoryCommand { get; set; }

		public MP3UtilitiesPageViewModel(IChooserDialogsService dialogsService)
		{
			_dialogsService = dialogsService ?? throw new ArgumentNullException(nameof(dialogsService));

			ChooseAlbumDirectoryCommand = new RelayCommand(OnChooseAlbumDirectory);
			ChooseAuthorAndTitleDirectoryCommand = new RelayCommand(OnChooseAuthorAndTitleDirectory);
		}

		private void OnChooseAlbumDirectory()
		{
			var directory = _dialogsService.ChooseDirectory("Choose a directory in which to set the album name");

			if (directory == null) return;

			Directory.EnumerateFiles(directory, $"*{MP3AudioSplitter.Mp3Extension}", SearchOption.AllDirectories).ForEach(mp3File =>
			{
				var tagLibFile = TagLib.File.Create(mp3File);
				tagLibFile.Tag.Album = AlbumName.IsNotNullOrEmpty() ? AlbumName : new FileInfo(mp3File).Directory.Name;
				tagLibFile.Save();
			});

			ShowInfoBox(InfoBoxType.Success, $"Successfully set album {(AlbumName.IsNotNullOrEmpty() ? $"({AlbumName}) " : "")}to files within the given directory!");
		}

		private void OnChooseAuthorAndTitleDirectory()
		{
			var directory = _dialogsService.ChooseDirectory("Choose a directory in which to set author and title based on file name");

			if (directory == null) return;

			Directory.EnumerateFiles(directory, $"*{MP3AudioSplitter.Mp3Extension}", SearchOption.AllDirectories).ForEach(mp3File =>
			{
				var info = Path
					.GetFileNameWithoutExtension(mp3File)
					.Split(AudioSplitDefinitionPageViewModel.TitleSeparator);

				var tagLibFile = TagLib.File.Create(mp3File);

				string title;

				if (info.Length == 1)
				{
					title = info[0].Trim();
				}
				else
				{
					title = info[1].Trim();
					var artists = info[0].Trim().Split('&').Select(a => a.Trim()).ToArray();

					tagLibFile.Tag.AlbumArtists = null;
					tagLibFile.Tag.AlbumArtists = new[] { artists[0] };
					tagLibFile.Tag.Performers = null;
					tagLibFile.Tag.Performers = artists;
				}

				tagLibFile.Tag.Title = title;
				tagLibFile.Save();
			});

			ShowInfoBox(InfoBoxType.Success, "Successfully set authors and titles to files within the given directory!");
		}
	}
}
