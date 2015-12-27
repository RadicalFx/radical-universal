using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radical.Windows.Presentation.ComponentModel;

namespace Radical.Windows.Presentation.Services
{
	class BultinKnownTypesProvider : IKnownTypesProvider
	{
		public IEnumerable<Type> GetKnownTypes()
		{
			yield return typeof( Dictionary<String, StorageItem> );
			yield return typeof( StorageLocation );
			yield return typeof( StorageItem );
			yield return typeof( JournalEntry );
			yield return typeof( JournalEntry[] );
		}
	}
}
