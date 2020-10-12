namespace AudioDownloader.WpfClient
{
	public class ConfigurationKeys : Braco.Services.Media.ConfigurationKeys
	{
		public const string Volume = nameof(Volume);
		public const string Language = nameof(Language);
		public const string AudioDownloadDirectory = nameof(AudioDownloadDirectory);
		public const string DeleteVideoAfterMP3Conversion = nameof(DeleteVideoAfterMP3Conversion);
		public const string DeleteAudioSplitSourceFile = nameof(DeleteAudioSplitSourceFile);
	}
}
