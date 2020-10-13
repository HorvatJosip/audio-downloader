using Braco.Services;
using Braco.Services.Media;
using Braco.Utilities.Wpf;

namespace AudioDownloader.WpfClient
{
	class AppInitializer : Initializer
	{
		public AppInitializer() : base(addEverything: false)
		{
			AddServiceSetups
			(
				// Braco.Services
				typeof(FileConfigurationServiceSetup),
				typeof(MapperSetup),
				typeof(PathManagerSetup),
				typeof(ResourceManagerSetup),
				typeof(Braco.Services.OtherServicesSetup),
				typeof(WindowsProcessStarterSetup),

				// Braco.Utilities.Wpf
				typeof(WpfDictionaryLocalizerSetup),
				typeof(ImageGetterSetup),
				typeof(ToolTipGetterSetup),
				typeof(ViewModelsSetup),
				typeof(Braco.Utilities.Wpf.OtherServicesSetup),

				// Braco.Services.Media
				typeof(ServicesSetup),

				// Custom
				typeof(AssemblyGetterSetup),
				typeof(MainWindowViewModelSetup)
			);

			AddPostServiceBuildInitializers(typeof(ResourceManagerInitializer));
		}
	}
}
