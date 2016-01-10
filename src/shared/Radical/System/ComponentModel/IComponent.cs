using System;
using System.Collections.Generic;
using System.Text;

namespace System.ComponentModel
{
    public interface IComponent: IDisposable
    {
        event EventHandler Disposed;
    }
}
