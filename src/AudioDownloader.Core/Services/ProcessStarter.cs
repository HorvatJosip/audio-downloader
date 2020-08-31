using AudioDownloader.Infrastructure;
using NLog;
using System;
using System.Diagnostics;

namespace AudioDownloader.Core
{
	public class ProcessStarter : IProcessStarter
	{
		private const string EXPLORER_PROCESS_NAME = "explorer.exe";

		private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

		public bool OpenDirectory(string directoryPath)
		{
			try
			{
				Process.Start(new ProcessStartInfo(EXPLORER_PROCESS_NAME, directoryPath)
				{
					UseShellExecute = true
				});

				return true;
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Couldn't open the containing folder {directoryPath}");

				return false;
			}
		}

		public bool OpenFile(string filePath)
		{
			try
			{
				Process.Start(new ProcessStartInfo(filePath)
				{
					UseShellExecute = true
				});

				return true;
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Couldn't start the file {filePath}");

				return false;
			}
		}
	}
}
