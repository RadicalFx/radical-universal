using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radical.ComponentModel;
using Radical.ComponentModel.Windows.Input;
using Radical.Reflection;

namespace Radical.Windows.CommandBuilders
{
    public class CommandData
    {
        public Object DataContext;
        public LateBoundVoidMethod FastDelegate;

        //public Fact Fact;
        public BooleanFact BooleanFact;

        public Boolean HasParameter;
        public Type ParameterType;

        //public KeyBindingAttribute[] KeyBindings;
        public CommandDescriptionAttribute Description;

        public IMonitor Monitor;
    }
}
