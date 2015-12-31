using System;
using Windows.ApplicationModel;

namespace Radical.Windows.Behaviors
{
	public static class DesignTimeHelper
	{
		/// <summary>
		/// Gets a value indicating whether we are in design mode.
		/// </summary>
		/// <returns><c>True</c> if we are in design mode, otherwise <c>false</c>.</returns>
		public static Boolean GetIsInDesignMode()
		{
			return DesignMode.DesignModeEnabled;
        }
	}
}
