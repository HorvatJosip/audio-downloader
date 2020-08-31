using System.IO;

namespace AudioDownloader.Infrastructure
{
	public class AudioDownloadConfiguration
	{
		public DirectoryInfo VideosDirectory { get; set; }
		public DirectoryInfo AudioDirectory { get; set; }
		public bool DeleteVideoAfterConversion { get; set; }

		public AudioDownloadConfiguration()
		{

		}

		public AudioDownloadConfiguration(DirectoryInfo videosDirectory, DirectoryInfo audioDirectory, bool deleteVideoAfterConversion)
		{
			VideosDirectory = videosDirectory;
			AudioDirectory = audioDirectory;
			DeleteVideoAfterConversion = deleteVideoAfterConversion;
		}
	}
}
