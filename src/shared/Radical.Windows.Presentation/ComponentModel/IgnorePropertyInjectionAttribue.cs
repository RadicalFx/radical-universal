﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radical.Windows.Presentation.ComponentModel
{
	/// <summary>
	/// Applied to properties prevents the IoC container to inject/intercept properties.
	/// </summary>
	[AttributeUsage( AttributeTargets.Property )]
	public class IgnorePropertyInjectionAttribue : Attribute
	{
	}
}
