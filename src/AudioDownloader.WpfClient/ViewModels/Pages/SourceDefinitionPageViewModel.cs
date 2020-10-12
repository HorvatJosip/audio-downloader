﻿using Braco.Services.Media;
using Braco.Services.Media.Abstractions;
using Braco.Utilities;
using Braco.Utilities.Extensions;
using Braco.Utilities.Wpf;
using System;
using System.IO;
using System.Threading;
using System.Windows.Input;

namespace AudioDownloader.WpfClient
{
	public class SourceDefinitionPageViewModel : PageViewModel
	{
		private const int downloadInfoCallback = 1;
		private const int videoDownloadedCallback = 2;

		private readonly IMediaDownloader _audioDownloader;
		private readonly char[] _invalidFileNameChars;

		private CancellationTokenSource _cancellationTokenSource;
		private int _callbackCounter;

		public string YouTubeLink { get; set; }
		public string SongTitle { get; set; }

		public bool IsDownloading { get; set; }
		public bool HasDownloaded { get; set; }

		public ICommand DownloadCommand { get; }
		public ICommand CancelCommand { get; }

		public SourceDefinitionPageViewModel(IMediaDownloader audioDownloader)
		{
			_audioDownloader = audioDownloader ?? throw new ArgumentNullException(nameof(audioDownloader));
			_invalidFileNameChars = Path.GetInvalidFileNameChars();

			DownloadCommand = new RelayCommand(OnDownload);
			CancelCommand = new RelayCommand(OnCancel);
		}

		private async void OnDownload()
		{
			if (IsDownloading) return;

			IsDownloading = true;
			_cancellationTokenSource = new CancellationTokenSource();
			
			var response = await _audioDownloader.DownloadAsync
			(
				request: new MediaDownloadRequest
				{
					DataCallback = DataCallback,
					Directory = new DirectoryInfo(_config[ConfigurationKeys.AudioDownloadDirectory]),
					DownloadInfo = new RemoteResourceDownloadInfo
					{
						ChunkSize = 1024,
						Uri = YouTubeLink
					}
				},
				cancellationToken: _cancellationTokenSource.Token
			);

			if (response.Finished)
			{
				if (_config.Get<bool>(ConfigurationKeys.DeleteVideoAfterMP3Conversion))
				{
					var videosDirectory = _config[ConfigurationKeys.VideoDownloadDirectory];

					Directory
						.GetFiles(videosDirectory, $"{response.Data.Title}.*", SearchOption.TopDirectoryOnly)
						.ForEach(file => File.Delete(file));
				}

				_windowService.ChangePage<AudioSplitDefinitionPageViewModel>(response);
			}
			else if (response.UnfinishedReason != MediaDownloadResponse.RequestCancelled)
			{
				_windowService.ShowErrorInInfoBox(_localizer[response.UnfinishedReason]);
			}

			IsDownloading = false;
		}

		private void OnCancel()
		{
			try
			{
				_cancellationTokenSource.Cancel();
			}
			catch (Exception ex)
			{
				_windowService.ShowErrorInInfoBox(_localizer.Format(LocalizationKeys.SourceDefinition_CancellationProcessError, new[] { ex?.Message }));
			}

			IsDownloading = false;
			HasDownloaded = false;
			SongTitle = null;
			_callbackCounter = 0;
		}

		private void DataCallback(MediaData mediaInfo)
		{
			switch (++_callbackCounter)
			{
				case downloadInfoCallback:
					var file = new FileInfo(Path.Combine
					(
						_config[ConfigurationKeys.AudioDownloadDirectory],
						$"{mediaInfo.Title}{MP3AudioSplitter.Mp3Extension}"
					));

					if (file.Exists)
					{
						OnCancel();

						_windowService.ChangePage<AudioSplitDefinitionPageViewModel>(new MediaDownloadResponse
						{
							File = file,
							Data = mediaInfo
						});
					}
					else
					{
						SongTitle = mediaInfo.Title;
					}
					break;

				case videoDownloadedCallback:
					HasDownloaded = true;
					break;
			}
		}

		public override void OnLoaded(WindowViewModel windowVM, object pageData, PageViewModel previousPage)
		{
			if (!(previousPage is SettingsPageViewModel))
			{
				base.OnLoaded(windowVM, pageData, previousPage);

				IsDownloading = false;
				HasDownloaded = false;
				YouTubeLink = null;
				SongTitle = null;
				_callbackCounter = 0;
			}
		}
	}
}