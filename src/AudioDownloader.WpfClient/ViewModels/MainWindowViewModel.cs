using Braco.Utilities;
using System.Globalization;
using System.Threading;

namespace AudioDownloader.WpfClient
{
	public class MainWindowViewModel : Braco.Utilities.Wpf.Controls.Windows.MainWindowViewModel
	{
		public MainWindowViewModel()
		{
			SettingsCommand = new RelayCommand(OnSettings);
		}

		private void OnSettings()
		{
			_windowService.ChangePage<SettingsPageViewModel>();
		}

		protected override void OnLanguageChanged(string culture)
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
		}
	}
}
