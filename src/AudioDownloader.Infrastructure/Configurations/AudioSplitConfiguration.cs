namespace AudioDownloader.Infrastructure
{
	public class AudioSplitConfiguration
	{
		public bool DeleteSourceFile { get; set; }

		public AudioSplitConfiguration()
		{

		}

		public AudioSplitConfiguration(bool deleteSourceFile)
		{
			DeleteSourceFile = deleteSourceFile;
		}
	}
}
