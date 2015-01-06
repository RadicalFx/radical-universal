using System;
using System.Linq;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Radical.ComponentModel;
using Radical.ComponentModel.Messaging;
using Radical.Messaging;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Radical.Linq;

namespace Radical.Windows.Presentation.Boot.Installers
{
	[Export( typeof( IPuzzleSetupDescriptor ) )]
	public class DefaultDescriptor : IPuzzleSetupDescriptor
	{
		public async Task Setup( IPuzzleContainer container, Func<IEnumerable<TypeInfo>> knownTypesProvider )
		{
			await Task.Run( () => 
			{
				var conventions = container.Resolve<BootstrapConventions>();
				var allTypes = knownTypesProvider();

                allTypes.Where( t => conventions.IsMessageHandler( t ) && !conventions.IsExcluded( t ) )
					.Select( t => new
					{
						Contract = conventions.SelectMessageHandlerContract( t ),
						Implementation = t
					} )
					.ForEach( descriptor =>
					{
						container.Register(
							EntryBuilder.For( descriptor.Contract )
								.ImplementedBy( descriptor.Implementation )
						);
					} );

				container.Register(
					EntryBuilder.For<Application>()
						.UsingFactory( () => Application.Current )
						.WithLifestyle( Lifestyle.Singleton )
				);

				container.Register(
					EntryBuilder.For<IMessageBroker>()
						.ImplementedBy<MessageBroker>()
						.WithLifestyle( Lifestyle.Singleton )
                        .Overridable()
				);
			} );
		}
	}
}