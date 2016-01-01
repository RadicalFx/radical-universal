//extern alias tpx;

namespace RadicalTests.ChangeTracking
{
    using Radical.ComponentModel.ChangeTracking;
    using Radical.Model;
    using System;

    class Person : MementoEntity, IDisposable
	{
		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );

			if( disposing )
			{
				//if( this.Site != null && this.Site.Container != null )
				//{
				//    this.Site.Container.Remove( this );
				//}
			}

			this.OnDisposed();
		}
        
		/*
		 * Non possiamo usare la EventHaldlerList
		 * perchè in fase di dispose ce la perdiamo...
		 */
		public event EventHandler Disposed;

		protected virtual void OnDisposed()
		{
			if( this.Disposed != null )
			{
				this.Disposed( this, EventArgs.Empty );
			}
		}

		public Person( IChangeTrackingService memento )
			: this( memento, true )
		{

		}

		public Person( IChangeTrackingService memento, Boolean registerAsTransient )
			: base( memento, registerAsTransient )
		{
			this.nameRejectCallback = ( pcr ) =>
			{
				this.CacheChangeOnRejectCallback( "property-name", this.Name, nameRejectCallback, null, pcr );
				this._name = pcr.CachedValue;
			};
		}

        TransientRegistration transientRegistration = TransientRegistration.AsTransparent;

        public Person( IChangeTrackingService memento, ChangeTrackingRegistration registration, TransientRegistration transientRegistration )
            : base( memento, registration )
        {
            this.transientRegistration = transientRegistration;

            this.nameRejectCallback = ( pcr ) =>
            {
                this.CacheChangeOnRejectCallback( "property-name", this.Name, nameRejectCallback, null, pcr );
                this._name = pcr.CachedValue;
            };
        }

        protected override void OnRegisterTransient( TransientRegistration transientRegistration )
        {
            base.OnRegisterTransient( this.transientRegistration );
        }

		private readonly RejectCallback<String> nameRejectCallback = null;
		private String _name = String.Empty;

		public String Name
		{
			get { return this._name; }
			set
			{
				if( value != this.Name )
				{
					this.CacheChange( "property-name", this.Name, nameRejectCallback );
					this._name = value;
				}
			}
		}
	}
}
