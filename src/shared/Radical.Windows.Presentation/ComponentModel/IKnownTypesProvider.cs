using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radical.Windows.Presentation.ComponentModel
{
    public interface IKnownTypesProvider
    {
        IEnumerable<Type> GetKnownTypes();
    }
}
