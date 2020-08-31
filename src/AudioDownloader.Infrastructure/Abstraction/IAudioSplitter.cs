using System.Threading.Tasks;

namespace AudioDownloader.Infrastructure
{
	public interface IAudioSplitter
	{
		AudioSplitConfiguration Config { get; }

		Task<AudioSplitResult> SplitAsync(AudioSplitOptions options);
	}
}
