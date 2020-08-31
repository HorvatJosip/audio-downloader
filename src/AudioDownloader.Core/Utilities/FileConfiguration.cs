using AudioDownloader.Infrastructure;
using NLog;
using System;
using System.IO;

namespace AudioDownloader.Core
{
	public class FileConfiguration
	{
		private readonly string _filePath;
		private readonly ILogger _logger;

		public AudioDownloadConfiguration AudioDownloadConfiguration { get; private set; }
		public AudioSplitConfiguration AudioSplitConfiguration { get; private set; }

		public FileConfiguration(string filePath)
		{
			_filePath = filePath;

			_logger = LogManager.GetCurrentClassLogger();
		}

		/// <summary>
		/// Tries to load the configuration from the file. If there isn't one,
		/// default values will be provided.
		/// </summary>
		public FileConfiguration Load()
		{
			AudioDownloadConfiguration = new AudioDownloadConfiguration();
			AudioSplitConfiguration = new AudioSplitConfiguration();

			try
			{
				if (File.Exists(_filePath))
				{
					var lines = File.ReadAllLines(_filePath);

					AudioDownloadConfiguration.VideosDirectory = new DirectoryInfo(lines[0]);
					AudioDownloadConfiguration.AudioDirectory = new DirectoryInfo(lines[1]);
					AudioDownloadConfiguration.DeleteVideoAfterConversion = bool.Parse(lines[2]);
					AudioSplitConfiguration.DeleteSourceFile = bool.Parse(lines[3]);
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
			}

			AudioDownloadConfiguration.VideosDirectory = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
			AudioDownloadConfiguration.AudioDirectory = new DirectoryInfo(Path.GetTempPath());
			AudioDownloadConfiguration.DeleteVideoAfterConversion = true;
			AudioSplitConfiguration.DeleteSourceFile = true;

			return this;
		}

		/// <summary>
		/// Tries to save the configuration into the file.
		/// </summary>
		/// <returns>If it was saved or not</returns>
		public bool Save()
		{
			try
			{
				File.WriteAllLines(_filePath, new[]
				{
					AudioDownloadConfiguration.VideosDirectory.FullName,
					AudioDownloadConfiguration.AudioDirectory.FullName,
					AudioDownloadConfiguration.DeleteVideoAfterConversion.ToString(),
					AudioSplitConfiguration.DeleteSourceFile.ToString()
				});

				return true;
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
				return false;
			}
		}
	}
}
