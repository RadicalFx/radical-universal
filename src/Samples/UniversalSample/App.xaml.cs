using Radical;
using Windows.UI.Xaml;

namespace UniversalSample
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        ApplicationBootstrapper<Presentation.MainView> bootstrapper;

        public App()
        {
            this.InitializeComponent();

            this.bootstrapper = new ApplicationBootstrapper<Presentation.MainView>();
        }
    }
}
