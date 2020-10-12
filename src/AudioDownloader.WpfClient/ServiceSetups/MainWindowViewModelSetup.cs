using Braco.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AudioDownloader.WpfClient
{
	class MainWindowViewModelSetup : ISetupService
	{
		public string ConfigurationSection { get; }

		public void Setup(IServiceCollection services, IConfiguration configuration, IConfigurationSection section)
		{
			services.AddSingleton<Braco.Utilities.Wpf.Controls.Windows.MainWindowViewModel, MainWindowViewModel>();
		}
	}
}
