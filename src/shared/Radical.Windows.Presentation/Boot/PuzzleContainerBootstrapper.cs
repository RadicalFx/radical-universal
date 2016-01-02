using Radical.ComponentModel;
using Radical.Windows.Presentation.ComponentModel;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.Reflection;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Radical.Windows.Presentation.Navigation.Hosts;

namespace Radical.Windows.Presentation.Boot
{
    class PuzzleContainerBootstrapper : IContainerBootstrapper
    {
        IPuzzleContainer container;
        AbstractApplicationBootstrapper owner;

        [ImportMany]
        public IEnumerable<IPuzzleSetupDescriptor> Installers { get; set; }

        public IServiceProvider CreateServiceProvider(AbstractApplicationBootstrapper owner)
        {
            this.container = new PuzzleContainer();
            this.owner = owner;
            var facade = new PuzzleContainerServiceProviderFacade(this.container);

            this.container.Register(EntryBuilder.For<AbstractApplicationBootstrapper>()
                .UsingInstance(owner));
            this.container.Register(EntryBuilder.For<IPuzzleContainer>()
                .UsingInstance(this.container));
            this.container.Register(EntryBuilder.For<IServiceProvider>()
                .UsingInstance(facade));
            this.container.Register(EntryBuilder.For<BootstrapConventions>()
                .UsingInstance(new BootstrapConventions()));

            var view = CoreApplication.GetCurrentView();
            var dispatcher = view.CoreWindow.Dispatcher;

            this.container.Register(
                    EntryBuilder.For<CoreDispatcher>()
                        .UsingInstance(dispatcher)
            );

            this.container.AddFacility<SubscribeToMessageFacility>();

            return facade;
        }


        public async Task OnCompositionContainerComposed(CompositionHost container, Func<IEnumerable<TypeInfo>> boottimeTypesProvider)
        {
            await this.container.SetupWith(boottimeTypesProvider, this.Installers.ToArray());

            if(!this.container.IsRegistered<NavigationHost>() && this.owner.Host != null)
            {
                this.container.Register(
                    EntryBuilder.For<NavigationHost>()
                        .UsingInstance(this.owner.Host)
                        .WithLifestyle(Lifestyle.Singleton)
                );
            }
        }
    }
}
