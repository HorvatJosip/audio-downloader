using AutoMapper;
using Braco.Services.Media;
using Braco.Services.Media.Abstractions;
using Braco.Utilities;
using Braco.Utilities.Extensions;
using Braco.Utilities.Wpf;
using NAudio.Wave;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AudioDownloader.WpfClient
{
	[AutoMap(sourceType: typeof(MediaDownloadResponse))]
	public class AudioSplitDefinitionPageViewModel : PageViewModel
	{
		private readonly IMediaSplitter _audioSplitter;
		private readonly IWavePlayer _soundPlayer;

		private Mp3FileReader _mp3;
		private WaveChannel32 _waveChannel;

		public MediaData Data { get; set; }
		public FileInfo File { get; set; }

		#region Audio Title

		private string _audioTitle;
		public string AudioTitle
		{
			get => _audioTitle;
			set
			{
				if (Equals(value, _audioTitle)) return;

				_audioTitle = PathUtilities.GetFileNameWithoutInvalidChars(value);
			}
		} 

		#endregion

		public TimeSpan AudioDuration { get; set; }

		public double SliderMaximum { get; set; }

		public ObservableCollection<TimeRangeViewModel> SplitRanges { get; set; } = new ObservableCollection<TimeRangeViewModel>();

		public bool IsConfiguring { get; set; } = true;
		public bool IsPlaying { get; set; }

		#region Sound Volume

		private double _soundVolume;

		[Setting(Key = ConfigurationKeys.Volume, Load = false)]
		public double SoundVolume
		{
			get => _soundVolume;
			set
			{
				if (Equals(value, _soundVolume)) return;

				_soundVolume = value;

				if (_waveChannel != null)
				{
					_waveChannel.Volume = (float)value;
				}
			}
		}

		#endregion

		public TimeRangeViewModel StartEndRange { get; set; }

		public ICommand SplitCommand { get; }
		public ICommand SkipCommand { get; }
		public ICommand NormalizeTitleCommand { get; }
		public ICommand PlayCommand { get; }
		public ICommand PauseCommand { get; }
		public ICommand ReplayCommand { get; }
		public ICommand AddSplitRangeCommand { get; }
		public ICommand RemoveSplitRangeCommand { get; }

		public AudioSplitDefinitionPageViewModel(IMediaSplitter audioSplitter)
		{
			_audioSplitter = audioSplitter ?? throw new ArgumentNullException(nameof(audioSplitter));
			_soundPlayer = new DirectSoundOut();

			SplitCommand = new RelayCommand(OnSplit);
			SkipCommand = new RelayCommand(OnSkip);
			NormalizeTitleCommand = new RelayCommand(OnNormalizeTitle);
			PlayCommand = new RelayCommand(OnPlay);
			PauseCommand = new RelayCommand(OnPause);
			ReplayCommand = new RelayCommand(OnReplay);
			AddSplitRangeCommand = new RelayCommand(OnAddSplitRange);
			RemoveSplitRangeCommand = new RelayCommand<TimeRangeViewModel>(OnRemoveSplitRange);
		}

		private async void OnSplit()
		{
			IsConfiguring = false;

			OnPause();
			_mp3.Dispose();

			var request = new MediaSplitRequest
			{
				SourceFile = File,
				DestinationFile = new FileInfo(Path.Combine(File.DirectoryName, $"{AudioTitle}{MP3AudioSplitter.Mp3Extension}")),
				Start = StartEndRange.Start.Time,
				End = StartEndRange.End.Time,
				SplitRanges = SplitRanges.Select(viewModel =>
					new TimeRange(viewModel.Start.Time, viewModel.End.Time)
				).ToList()
			};

			var response = await _audioSplitter.SplitAsync(request, default);

			if (response.Finished)
			{
				if (_config.Get<bool>(ConfigurationKeys.DeleteAudioSplitSourceFile))
				{
					response.SourceFile.Delete();
				}

				_windowService.ChangePage<AudioProcessingResultPageViewModel>(response);
			}
			else
			{
				_windowService.ShowErrorInInfoBox(_localizer[response.UnfinishedReason]);
				InitializeSoundData();
			}
		}

		private void OnSkip()
		{
			IsConfiguring = false;

			_mp3.Dispose();

			if (File.Name != AudioTitle)
			{
				File.MoveTo(Path.Combine(File.DirectoryName, $"{AudioTitle}{MP3AudioSplitter.Mp3Extension}"));
			}

			_windowService.ChangePage<AudioProcessingResultPageViewModel>(new MediaSplitResponse
			{
				SplitFile = File
			});
		}

		private void OnNormalizeTitle()
		{
			if (AudioTitle.IsNullOrWhiteSpace()) return;

			var culture = CultureInfo.CurrentCulture ?? new CultureInfo(_localizer.Culture);

			if (culture == null)
			{
				_windowService.ShowErrorInInfoBox(_localizer[LocalizationKeys.AudioSplitDefinition_CurrentCultureError]);
				return;
			}

			AudioTitle = culture.TextInfo.ToTitleCase(AudioTitle.ToLower());
		}

		private void OnPlay()
		{
			IsPlaying = true;
			_soundPlayer.Play();
		}

		private void OnPause()
		{
			IsPlaying = false;
			_soundPlayer.Pause();
		}

		private void OnReplay()
		{
			_mp3.CurrentTime = TimeSpan.Zero;
			OnPlay();
		}

		private void OnAddSplitRange()
		{
			SplitRanges.Add(new TimeRangeViewModel(AudioDuration.TotalMilliseconds, OnTimeValueChanged, OnTimeDisplayChanged));
		}

		private void OnRemoveSplitRange(TimeRangeViewModel range)
		{
			SplitRanges.Remove(range);
		}

		public override void OnLoaded(WindowViewModel windowVM, object pageData, PageViewModel previousPage)
		{
			if (!(previousPage is SettingsPageViewModel))
			{
				base.OnLoaded(windowVM, pageData, previousPage);

				Task.Run(InitializeSoundData);

				AudioTitle = PathUtilities.GetFileNameWithoutInvalidChars(Data.Title);

				if (AudioDuration == TimeSpan.Zero && Data.Duration.HasValue)
				{
					AudioDuration = Data.Duration.Value;
				}
			}
		}

		private void InitializeSoundData()
		{
			_mp3 = new Mp3FileReader(File.FullName);

			AudioDuration = _mp3.TotalTime;
			SliderMaximum = AudioDuration.TotalMilliseconds;

			_waveChannel = new WaveChannel32(_mp3);
			_soundPlayer.Init(_waveChannel);
			SoundVolume = _config.Get<double>(ConfigurationKeys.Volume);

			StartEndRange = new TimeRangeViewModel(AudioDuration.TotalMilliseconds, OnTimeValueChanged, OnTimeDisplayChanged);

			IsConfiguring = true;
		}

		private void OnTimeValueChanged(double _, TimeSpan time)
		{
			_mp3.CurrentTime = time;
		}

		private void OnTimeDisplayChanged(string _, TimeSpan? time)
		{
			if (time.HasValue)
			{
				_mp3.CurrentTime = time.Value;
			}
		}
	}
}
