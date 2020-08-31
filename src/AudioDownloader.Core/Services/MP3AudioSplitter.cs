using AudioDownloader.Infrastructure;
using NAudio.Wave;
using NLog;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AudioDownloader.Core
{
	public class MP3AudioSplitter : IAudioSplitter
	{
		private const string MP3_EXTENSION = ".mp3";
		private const string TEMP_FILE_NAME_SUFFIX = "_OLD_";

		private readonly ILogger _logger;

		public AudioSplitConfiguration Config { get; }

		public MP3AudioSplitter(AudioSplitConfiguration config)
		{
			Config = config ?? throw new ArgumentNullException(nameof(config));

			_logger = LogManager.GetCurrentClassLogger();
		}

		public async Task<AudioSplitResult> SplitAsync(AudioSplitOptions options)
		{
			try
			{
				// Check if name changed
				var sameFileName = options.SourceFilePath == options.DestinationFilePath;

				string sourceFilePath;

				// If the name didn't change...
				if (sameFileName)
				{
					// Rename the file to something unique, but identifiable
					sourceFilePath = options.SourceFilePath.Replace
					(
						oldValue: MP3_EXTENSION,
						newValue: $"{TEMP_FILE_NAME_SUFFIX}{Guid.NewGuid()}{MP3_EXTENSION}"
					);

					File.Move(options.SourceFilePath, sourceFilePath);
				}

				// Otherwise...
				else
				{
					// Just keep the original name
					sourceFilePath = options.SourceFilePath;
				}

				// Create an MP3 reader
				using (var mp3Reader = new Mp3FileReader(sourceFilePath))
				// Open destination file in write mode
				using (var writer = File.OpenWrite(options.DestinationFilePath))
				{
					var first = true;

					while (true)
					{
						// Read the next frame
						var frame = mp3Reader.ReadNextFrame();

						// If there are no more frames, quit
						if (frame == null) break;

						// If this is the first chunk...
						if (first)
						{
							// Write it so that the file can be correct
							await writer.WriteChunkAsync(frame.RawData);

							first = false;
							continue;
						}

						var currentTime = mp3Reader.CurrentTime;

						// If we reached the end, stop writing to file
						if (options.End.HasValue && currentTime >= options.End) break;

						// If we aren't at start yet, skip
						if (options.Start.HasValue && currentTime < options.Start) continue;

						// If we are at one of the split ranges, skip
						if (options.SplitRanges?.Any(range => range.Contains(currentTime)) == true) continue;

						// Write the current frame to the file
						await writer.WriteChunkAsync(frame.RawData);
					}
				}

				// If we should delete the source file...
				if (Config.DeleteSourceFile)
				{
					// Delete it
					File.Delete(sourceFilePath);
				}

				// Return the result with output location
				return new AudioSplitResult(options.DestinationFilePath);
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
				return AudioSplitResult.FromError(ex.Message);
			}
		}
	}
}
