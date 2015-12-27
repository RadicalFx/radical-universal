using Radical.Windows.Presentation.Boot;
using Windows.UI.Xaml;

namespace UniversalSample
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        PuzzleApplicationBootstrapper<Presentation.MainView> bootstrapper;

        public App()
        {
            this.InitializeComponent();

            this.bootstrapper = new PuzzleApplicationBootstrapper<Presentation.MainView>();
        }
    }
}
