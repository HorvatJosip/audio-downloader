using AudioDownloader.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AudioDownloader.WinFormsClient
{
    public partial class AudioDownloader : Form
    {
        private readonly IAudioDownloader _downloader;
        private readonly IAudioSplitter _splitter;
        private readonly IProcessStarter _processStarter;
        private readonly Properties.Settings _settings;

        private string _downloadedAudioPath;

        public AudioDownloader(IAudioDownloader downloader, IAudioSplitter splitter, IProcessStarter processStarter)
        {
            InitializeComponent();
            SetUIState(UIState.LinkRequired);

            _downloader = downloader;

            _splitter = splitter;

            _processStarter = processStarter;

            _settings = Properties.Settings.Default;

            txtSaveLocation.Text = _settings.SaveLocation;
            cbOpenContainingDirectory.Checked = _settings.OpenContainingDirectory;
            cbRunAudioFile.Checked = _settings.RunAudioFile;

            Utils.AvoidSpecificCharacters(txtSaveLocation, Path.GetInvalidPathChars());
            Utils.AvoidSpecificCharacters(txtFileName, Path.GetInvalidFileNameChars());
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            if (_downloader.IsUriInValidFormat(txtLink.Text, out var link))
            {
                SetUIState(UIState.Downloading);

                var downloadResult = await _downloader.DownloadAsync(link);

                if (downloadResult.Error?.Length > 0)
                {
                    MessageBox.Show(downloadResult.Error, "An error occurred during the download process");
                    SetUIState(UIState.LinkRequired);

                    return;
                }

                txtFileName.Text = downloadResult.AudioTitle;

                _downloadedAudioPath = downloadResult.FilePath;

                SetUIState(UIState.Downloaded);
            }
            else
            {
                MessageBox.Show("You must provide a valid YouTube URL.");
                txtLink.Focus();
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            var splitOptions = new AudioSplitOptions();
            var splitOptionsAssigned = false;

            if (txtStart.TextLength > 0)
            {
                if (TryParseTimeSpan(txtStart, out var start, "You must provide the starting time in valid format (or not provide it at all)."))
                {
                    splitOptions.Start = start;
                }
                else
                {
                    txtStart.Focus();
                    return;
                }

                splitOptionsAssigned = true;
            }

            if (txtEnd.TextLength > 0)
            {
                if (TryParseTimeSpan(txtEnd, out var end, "You must provide the ending time in valid format (or not provide it at all)."))
                {
                    splitOptions.End = end;
                }
                else
                {
                    txtEnd.Focus();
                    return;
                }

                splitOptionsAssigned = true;
            }

            if (txtSplitRanges.TextLength > 0)
            {
                var cleanSplitRangesText = new string(txtSplitRanges.Text.Where(c => !char.IsWhiteSpace(c)).ToArray());
                var splitRangesAsText = cleanSplitRangesText.Split(',');

                var splitRanges = new List<AudioSplitRange>();

                var ordNo = 1;
                foreach (var range in splitRangesAsText)
                {
                    var split = range.Split('-');

                    if (split.Length != 2)
                    {
                        MessageBox.Show($"The range (ordinal number {ordNo}) doesn't have two 'times' split by '-'");

                        txtSplitRanges.Focus();
                        return;
                    }

                    if (!TryParseTimeSpan(split[0], out var splitStart, $"The range (ordinal number {ordNo}) doesn't have the starting time in valid format"))
                    {
                        txtSplitRanges.Focus();
                        return;
                    }

                    if (!TryParseTimeSpan(split[1], out var splitEnd, $"The range (ordinal number {ordNo}) doesn't have the ending time in valid format"))
                    {
                        txtSplitRanges.Focus();
                        return;
                    }

                    splitRanges.Add(new AudioSplitRange(splitStart, splitEnd));

                    ordNo++;
                }

                splitOptions.SplitRanges = splitRanges;

                splitOptionsAssigned = true;
            }

            if (splitOptionsAssigned)
            {
                splitOptions.SourceFilePath = _downloadedAudioPath;
                splitOptions.DestinationFilePath = Path.Combine(txtSaveLocation.Text, $"{txtFileName.Text}.mp3");

                var splitResult = await _splitter.SplitAsync(splitOptions);

                if (splitResult.Error?.Length > 0)
                {
                    MessageBox.Show(splitResult.Error);

                    return;
                }

                if (cbRunAudioFile.Checked)
                {
                    if (!_processStarter.OpenFile(splitResult.FilePath))
                    {
                        MessageBox.Show($"Couldn't open the file {splitResult.FilePath}");
                    }
                }

                if (cbOpenContainingDirectory.Checked)
                {
                    var containingFolder = Path.GetDirectoryName(splitResult.FilePath);

                    if (!_processStarter.OpenDirectory(containingFolder))
                    {
                        MessageBox.Show($"Couldn't open the containing folder {containingFolder}");
                    }
                }
            }

            SetUIState(UIState.LinkRequired);
        }

        private bool TryParseTimeSpan(TextBox textBox, out TimeSpan result, string errorMessage)
            => TryParseTimeSpan(textBox.Text, out result, errorMessage);

        private bool TryParseTimeSpan(string text, out TimeSpan result, string errorMessage)
        {
            var parseResult = TimeSpan.TryParse(text, out result);

            if (parseResult)
            {
                var colonCount = text.Count(c => c == ':');

                if (colonCount < 2)
                {
                    for (int i = 0; i < 2 - colonCount; i++)
                    {
                        text = "0:" + text;
                    }

                    result = TimeSpan.Parse(text);
                }
            }
            else
            {
                MessageBox.Show(errorMessage);
            }

            return parseResult;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetUIState(UIState.LinkRequired);
        }

        private void btnBrowseForSaveLocation_Click(object sender, EventArgs e)
        {
            using var browser = new FolderBrowserDialog();

            if (browser.ShowDialog() == DialogResult.OK)
            {
                ChangeSaveLocation(browser.SelectedPath);
            }
        }

        private void cbOpenContainingDirectory_CheckedChanged(object sender, EventArgs e)
        {
            _settings.OpenContainingDirectory = cbOpenContainingDirectory.Checked;
            _settings.Save();
        }

        private void cbRunAudioFile_CheckedChanged(object sender, EventArgs e)
        {
            _settings.RunAudioFile = cbRunAudioFile.Checked;
            _settings.Save();
        }

        private void txtSaveLocation_Leave(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtSaveLocation.Text))
            {
                if (MessageBox.Show("The given directory doesn't exist. Do you want to create it?", "Directory doesn't exist", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        Directory.CreateDirectory(txtSaveLocation.Text);
                        ChangeSaveLocation(txtSaveLocation.Text);
                        MessageBox.Show($"Directory {txtSaveLocation.Text} created successfully.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Directory {txtSaveLocation.Text} couldn't be created...{Environment.NewLine}{ex}");
                    }
                }
                else
                {
                    txtSaveLocation.Focus();
                }
            }
            else
            {
                ChangeSaveLocation(txtSaveLocation.Text);
            }
        }

        private void ChangeSaveLocation(string saveLocation)
        {
            txtSaveLocation.Text = saveLocation;

            _downloader.Config.AudioDirectory = new DirectoryInfo(saveLocation);
            _settings.SaveLocation = saveLocation;
            _settings.Save();
        }

        private void SetUIState(UIState state)
        {
            txtLink.Enabled = state == UIState.LinkRequired;
            btnDownload.Enabled = state == UIState.LinkRequired;

            txtFileName.Enabled = state == UIState.Downloaded;
            txtStart.Enabled = state == UIState.Downloaded;
            txtEnd.Enabled = state == UIState.Downloaded;
            txtSplitRanges.Enabled = state == UIState.Downloaded;
            btnSave.Enabled = state == UIState.Downloaded;
            btnCancel.Enabled = state == UIState.Downloaded;

            switch (state)
            {
                case UIState.LinkRequired:
                    txtFileName.Clear();
                    txtStart.Clear();
                    txtEnd.Clear();
                    txtSplitRanges.Clear();
                    break;

                case UIState.Downloading:
                    break;

                case UIState.Downloaded:
                    txtLink.Clear();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(state));
            }
        }

        private enum UIState
        {
            LinkRequired,
            Downloading,
            Downloaded,
        }
    }
}
