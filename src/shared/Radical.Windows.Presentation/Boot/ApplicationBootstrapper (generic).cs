using Radical.Windows.Presentation.Boot;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;

namespace Radical
{
    public class ApplicationBootstrapper<TShellView> : ApplicationBootstrapper where TShellView : UIElement
    {
        public ApplicationBootstrapper()
        {
            this.DefineHomeAs<TShellView>();
        }
    }
}
