namespace AudioDownloader.Infrastructure
{
	public class AudioSplitResult : Result<AudioSplitResult>
	{
		public string FilePath { get; set; }

		public AudioSplitResult()
		{

		}

		public AudioSplitResult(string filePath)
		{
			FilePath = filePath;
		}
	}
}
