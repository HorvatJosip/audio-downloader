using AudioDownloader.Core;
using AudioDownloader.Infrastructure;
using System;
using System.IO;
using System.Windows.Forms;
using VideoLibrary;

namespace AudioDownloader.WinFormsClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var settings = Properties.Settings.Default;

            if (string.IsNullOrWhiteSpace(settings.SaveLocation))
            {
                settings.SaveLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                settings.Save();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var audioDownloadConfiguration = new AudioDownloadConfiguration(
                new DirectoryInfo(Path.GetTempPath()), new DirectoryInfo(settings.SaveLocation), true);
            var audioSplitConfiguration = new AudioSplitConfiguration(true);

            var downloader = new YouTubeAudioDownloader(YouTube.Default, audioDownloadConfiguration);
            var splitter = new MP3AudioSplitter(audioSplitConfiguration);

            Application.Run(new AudioDownloader(downloader, splitter, new ProcessStarter()));
        }
    }
}
