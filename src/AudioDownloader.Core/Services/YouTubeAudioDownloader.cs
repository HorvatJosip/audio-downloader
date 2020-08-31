using AudioDownloader.Infrastructure;
using MediaToolkit;
using MediaToolkit.Model;
using NLog;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VideoLibrary;

namespace AudioDownloader.Core
{
	public class YouTubeAudioDownloader : IAudioDownloader
	{
		private const string REQUIRED_HOST_CHARS = "youtube";
		private const string URI_SCHEME_SEPARATOR = "://";
		private const string WWW = "www";

		private readonly ILogger _logger;
		private readonly YouTube _youTube;
        private readonly char[] _invalidFileNameChars;

        public YouTubeAudioDownloader(YouTube youTube, AudioDownloadConfiguration config)
		{
			_youTube = youTube ?? throw new ArgumentNullException(nameof(youTube));
			Config = config ?? throw new ArgumentNullException(nameof(config));

			_logger = LogManager.GetCurrentClassLogger();
            _invalidFileNameChars = Path.GetInvalidFileNameChars();
        }

        public AudioDownloadConfiguration Config { get; }

		public async Task<AudioDownloadResult> DownloadAsync(string uri)
		{
			// Make sure that the given URI is valid
			if (!IsUriInValidFormat(uri, out uri))
			{
				return AudioDownloadResult.FromError("You must provide a valid YouTube URI.");
			}

			YouTubeVideo videoWithBestBitRate;

			try
			{
				// Get all videos with given uri
				var videos = await _youTube.GetAllVideosAsync(uri);

				// Find one with the best bitrate
				videoWithBestBitRate = videos?.OrderByDescending(video => video.AudioBitrate).FirstOrDefault();
			}
			catch (Exception ex)
			{
				var errorMessage = $"An error occurred while trying to fetch video from {uri}: {ex.Message}";
				_logger.Error(ex, errorMessage);
				return AudioDownloadResult.FromError(errorMessage);
			}

			// Make sure it exists
			if (videoWithBestBitRate == null)
			{
				return AudioDownloadResult.FromError($"Couldn't find a video at given URI ({uri}).");
			}

			try
			{
				// Make sure that the given directories exist
				Config.VideosDirectory.Create();
				Config.AudioDirectory.Create();

				// Construct video destination
				var videoPath = Path.Combine(Config.VideosDirectory?.FullName, videoWithBestBitRate.FullName);

                // Make sure to remove invalid file name characters
                var cleanTitle = new string(videoWithBestBitRate.Title.Where(c => !_invalidFileNameChars.Contains(c)).ToArray());

                // Construct audio destination
                var audioPath = Path.Combine(Config.AudioDirectory?.FullName, $"{cleanTitle}.mp3");

				// Write the video to file
				File.WriteAllBytes(videoPath, await videoWithBestBitRate.GetBytesAsync());

				// Create conversion engine
				using var engine = new Engine();

				// Define input and output files for the engine
				var inputFile = new MediaFile(videoPath);
				var outputFile = new MediaFile(audioPath);

				// Read metadata from the input file
				engine.GetMetadata(inputFile);

				// Convert the video into audio file
				engine.Convert(inputFile, outputFile);

				// Return result
				return new AudioDownloadResult(audioPath, cleanTitle);
			}
			catch(Exception ex)
			{
				_logger.Error(ex);
				return AudioDownloadResult.FromError(ex.Message);
			}
		}

		public bool IsUriInValidFormat(string uri, out string formatted)
		{
			formatted = uri;

			if(!string.IsNullOrWhiteSpace(formatted))
			{
				if (!formatted.ToLower().StartsWith(Uri.UriSchemeHttp))
				{
					formatted = $"{Uri.UriSchemeHttps}{URI_SCHEME_SEPARATOR}{formatted}";
				}

				if (!formatted.ToLower().Contains(WWW))
				{
					formatted = formatted.Insert(formatted.IndexOf(URI_SCHEME_SEPARATOR) + URI_SCHEME_SEPARATOR.Length, $"{WWW}.");
				}

				if(Uri.TryCreate(formatted, UriKind.Absolute, out var createdUri))
				{
					return 
						(createdUri.Scheme == Uri.UriSchemeHttp ||
						createdUri.Scheme == Uri.UriSchemeHttps) &&
						REQUIRED_HOST_CHARS.All(character => createdUri.Host.Contains(character));
				}
			}

			return false;
		}
	}
}
