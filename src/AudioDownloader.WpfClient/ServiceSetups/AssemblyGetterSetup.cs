using Braco.Services;
using Braco.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AudioDownloader.WpfClient
{
	class AssemblyGetterSetup : ISetupService
	{
		public string ConfigurationSection { get; }

		public void Setup(IServiceCollection services, IConfiguration configuration, IConfigurationSection section)
		{
			services.AddSingleton(new AssemblyGetter(this));
		}
	}
}
