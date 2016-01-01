//extern alias tpx;

using Topics.Radical.Model;
using Topics.Radical.ComponentModel.ChangeTracking;

namespace RadicalTests.ChangeTracking
{

	class PersonCollection : MementoEntityCollection<Person>
	{
		public PersonCollection( IChangeTrackingService memento )
			: base()
		{
			this.Memento = memento;
		}
	}
}
