using Radical.Windows.Presentation.Boot;
using System;
using System.Collections.Generic;
using System.Composition.Hosting;
using System.Reflection;
using System.Threading.Tasks;

namespace Radical.Windows.Presentation.ComponentModel
{
    public interface IContainerBootstrapper
    {
        IServiceProvider CreateServiceProvider(AbstractApplicationBootstrapper owner);
        Task OnCompositionContainerComposed(CompositionHost container, Func<IEnumerable<TypeInfo>> boottimeTypesProvider);
    }
}