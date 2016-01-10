using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

using Radical.ComponentModel;
using Radical.Linq;
using Radical.Validation;
using System.Linq.Expressions;

namespace Radical.Model
{
    partial class EntityCollection<T>
    {

        /// <summary>
        /// Represents the method that handles the <see cref="E:System.ComponentModel.IComponent.Disposed"/> event of a component.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// Called to raise the Disposed event.
        /// </summary>
        protected virtual void OnDisposed()
        {
            if( this.Disposed != null )
            {
                this.Disposed( this, EventArgs.Empty );
            }
        }
    }
}