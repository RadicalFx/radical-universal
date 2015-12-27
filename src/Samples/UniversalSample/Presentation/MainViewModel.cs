using Radical.Windows.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSample.Presentation
{
    class MainViewModel : AbstractViewModel
    {
        public MainViewModel()
        {
            this.SetInitialPropertyValue(() => this.Cheers, "Hi, there!");
        }

        public String Cheers {
            get { return this.GetPropertyValue(() => this.Cheers); }
            set { this.SetPropertyValue(() => this.Cheers, value); } }
    }
}
