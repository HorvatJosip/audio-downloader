using System.Threading.Tasks;

namespace AudioDownloader.Infrastructure
{
	public interface IAudioDownloader
	{
		AudioDownloadConfiguration Config { get; }

		bool IsUriInValidFormat(string uri, out string formatted);

		Task<AudioDownloadResult> DownloadAsync(string uri);
	}
}
