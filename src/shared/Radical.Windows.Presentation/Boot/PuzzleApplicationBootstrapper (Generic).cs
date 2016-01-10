using System;
using System.Windows;
using Radical.Windows.Presentation.ComponentModel;
using Windows.UI.Xaml;

namespace Radical.Windows.Presentation.Boot
{
    public class PuzzleApplicationBootstrapper<TShellView> : 
        PuzzleApplicationBootstrapper 
        where TShellView : UIElement
    {
        public PuzzleApplicationBootstrapper()
        {
            this.DefineHomeAs<TShellView>();
        }
    }
}
