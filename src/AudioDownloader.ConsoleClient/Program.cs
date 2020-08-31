using AudioDownloader.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static AudioDownloader.ConsoleClient.ConsoleHelpers;
using static System.Console;

namespace AudioDownloader.ConsoleClient
{
	class Program
	{
		private readonly IAudioDownloader _downloader;
		private readonly IAudioSplitter _splitter;
		private readonly IProcessStarter _processStarter;
		private readonly char[] _invalidChars;

		public Program(IAudioDownloader downloader, IAudioSplitter splitter, IProcessStarter processStarter)
		{
			_downloader = downloader;

			_splitter = splitter;

			_processStarter = processStarter;

			_invalidChars = Path.GetInvalidFileNameChars();
		}

		public async Task Run(string[] arguments)
		{
			var firstTime = true;

			while (true)
			{
				Clear();
				PrintHeader("Audio Downloader", false);

				string uri = null;

				if (firstTime)
				{
					if (!(arguments?.Length > 0 && _downloader.IsUriInValidFormat(arguments[0], out uri)))
					{
						WriteLine("You must provide a valid URI to the audio!");

						uri = null;
					}

					firstTime = false;
				}

				var splitOptions = new AudioSplitOptions();
				var splitOptionsAssigned = false;

				if (uri != null)
				{
					if (arguments.Length > 1)
					{
						ParseSplitOptionsFromArgs(splitOptions, arguments);
						splitOptionsAssigned = true;
					}
				}
				else
				{
					Read<string>("Enter the audio URI: ", input => _downloader.IsUriInValidFormat(input, out uri));
				}

				PrintHeader($"Downloading: {uri}");

				var downloadResult = await _downloader.DownloadAsync(uri);

				if (downloadResult.Error == null)
				{
					WriteLine($"{downloadResult.AudioTitle} downloaded.");

					if (!splitOptionsAssigned)
					{
						if (Confirm("Do you want to split the audio? (y/n)"))
						{
							AskForSplitOptions(splitOptions);
							splitOptionsAssigned = true;
						}
					}

					var askToOpen = true;

					var destinationFilePath = downloadResult.FilePath;

					if (splitOptionsAssigned)
					{
						if (Confirm($"Do you want to rename file from \"{Path.GetFileNameWithoutExtension(destinationFilePath)}\"? (y/n)"))
						{
							var newFileName = Read<string>("Enter new file name: ", input => !input.Any(character => _invalidChars.Contains(character)));

							destinationFilePath = Path.Combine(Path.GetDirectoryName(destinationFilePath), $"{newFileName}.mp3");
						}

						splitOptions.SourceFilePath = downloadResult.FilePath;
						splitOptions.DestinationFilePath = destinationFilePath;

						PrintHeader("Splitting");

						var splitResult = await _splitter.SplitAsync(splitOptions);

						if (splitResult.Error == null)
						{
							WriteLine(splitResult.FilePath);
						}
						else
						{
							WriteLine("An error occurred during the audio split process:");
							WriteLine(splitResult.Error);
							WriteLine();

							askToOpen = false;
						}
					}

					if (askToOpen)
					{
						if (Confirm("Do you want to run the file? (y/n)"))
						{
							if (!_processStarter.OpenFile(destinationFilePath))
							{
								WriteLine($"Couldn't start the file {destinationFilePath}");
							}
						}
						else if (Confirm("Do you want to open the containing folder? (y/n)"))
						{
							var containingFolder = Path.GetDirectoryName(destinationFilePath);

							if (!_processStarter.OpenDirectory(containingFolder))
							{
								WriteLine($"Couldn't open the containing folder {containingFolder}");
							}
						}
					}
				}
				else
				{
					WriteLine("An error occurred during the audio download process:");
					WriteLine(downloadResult.Error);
					WriteLine();
				}

				Write("Press any key to process another audio file or 'q' to quit...");

				if (ReadKey(true).Key == ConsoleKey.Q)
				{
					WriteLine(Environment.NewLine);
					break;
				}
			}
		}

		private void AskForSplitOptions(AudioSplitOptions splitOptions)
		{
			if (Confirm("Do you want to specify starting point? (y/n)"))
			{
				splitOptions.Start = ReadTimeSpan("Enter starting point: ");
			}

			if (Confirm("Do you want to specify ending point? (y/n)"))
			{
				splitOptions.End = ReadTimeSpan("Enter ending point: ");
			}

			if (Confirm("Do you want to specify some split ranges? (y/n)"))
			{
				var splitRanges = new List<AudioSplitRange>();

				while (true)
				{
					var start = ReadTimeSpan("Enter range start: ");
					var end = ReadTimeSpan("Enter range end: ");

					splitRanges.Add(new AudioSplitRange(start, end));

					if (!Confirm("Do you want to add another split range? (y/n)")) break;
				}

				splitOptions.SplitRanges = splitRanges;
			}
		}

		private void ParseSplitOptionsFromArgs(AudioSplitOptions splitOptions, string[] args)
		{
			var providedSplitArgs = args
				.Skip(1)
				.Select(arg =>
				{
					var success = TimeSpan.TryParse(arg, out TimeSpan parsed);

					return (parsed, success);
				})
				.Where(parsed => parsed.success)
				.ToList();

			var processed = 0;

			var i = 0;
			while (i < providedSplitArgs.Count)
			{
				var (time, success) = providedSplitArgs[i];

				if (processed == 0)
				{
					if (success)
					{
						splitOptions.Start = time;

						processed++;
					}
				}
				else
				{
					if (success)
					{
						splitOptions.End = time;
					}

					processed++;
					break;
				}

				i++;
			}

			if (processed == 2)
			{
				var splitRanges = new List<AudioSplitRange>();

				var validArgs = providedSplitArgs.Where(arg => arg.success).Select(arg => arg.parsed).ToList();

				for (int j = i; j < validArgs.Count - 1; j += 2)
				{
					splitRanges.Add(new AudioSplitRange(validArgs[j], validArgs[j + 1]));
				}

				if (splitRanges.Count > 0)
				{
					splitOptions.SplitRanges = splitRanges;
				}
			}
		}
	}
}
