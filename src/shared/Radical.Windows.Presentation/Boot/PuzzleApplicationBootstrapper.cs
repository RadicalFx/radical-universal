using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Radical.ComponentModel;
using Radical.ComponentModel.Messaging;
using Radical.Windows.Presentation.Navigation.Hosts;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Radical.Windows.Presentation.Boot
{
	public class PuzzleApplicationBootstrapper : AbstractApplicationBootstrapper
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="PuzzleApplicationBootstrapper"/> class.
        /// </summary>
        protected PuzzleApplicationBootstrapper()
		{

		}
	}
}
