namespace AudioDownloader.Infrastructure
{
	public interface IProcessStarter
	{
		bool OpenDirectory(string directoryPath);

		bool OpenFile(string filePath);
	}
}
