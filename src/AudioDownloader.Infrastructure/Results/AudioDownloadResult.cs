namespace AudioDownloader.Infrastructure
{
	public class AudioDownloadResult : Result<AudioDownloadResult>
	{
		public string FilePath { get; set; }
		public string AudioTitle { get; set; }

		public AudioDownloadResult()
		{

		}

		public AudioDownloadResult(string filePath, string audioTitle)
		{
			FilePath = filePath;
			AudioTitle = audioTitle;
		}
	}
}
