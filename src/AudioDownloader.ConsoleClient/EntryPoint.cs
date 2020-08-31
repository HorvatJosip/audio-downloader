using AudioDownloader.Core;
using System.Threading.Tasks;
using VideoLibrary;

namespace AudioDownloader.ConsoleClient
{
	public static class EntryPoint
	{
		public static async Task Main(string[] args)
		{
			var configuration = new FileConfiguration("temp.txt").Load();

			var downloader = new YouTubeAudioDownloader(YouTube.Default, configuration.AudioDownloadConfiguration);
			var splitter = new MP3AudioSplitter(configuration.AudioSplitConfiguration);

			var program = new Program(downloader, splitter, new ProcessStarter());

			await program.Run(args);
		}
	}
}
