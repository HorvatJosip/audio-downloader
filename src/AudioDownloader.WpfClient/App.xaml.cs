using Braco.Services;
using Braco.Utilities.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace AudioDownloader.WpfClient
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			Initializer.RunSpecificInitializer<AppInitializer>();

			StyleUtilities.OverrideStyles(typeof(Window), typeof(Page));

			var windowService = (WindowService)DI.Get<IWindowService>();

			windowService.Open<MainWindowViewModel, SourceDefinitionPageViewModel>();

			DI.Configuration.Load();

			while (!DI.Localizer.ChangeLanguage(DI.Configuration[ConfigurationKeys.Language])) ;

			Current.MainWindow = windowService.CurrentWindow.UI;
			Current.MainWindow.ShowDialog();
		}

		protected override void OnExit(ExitEventArgs e)
		{
			DI.Configuration.Save();

			base.OnExit(e);
		}
	}
}
