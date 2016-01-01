﻿//extern alias tpx;

using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Rhino.Mocks;
using Topics.Radical.ComponentModel.ChangeTracking;


namespace RadicalTests
{
	[TestClass()]
	public class ChangeCommittedArgsTests : ChangeArgsTests
	{
		protected override ChangeEventArgs<T> CreateMock<T>( object entity, T cachedValue, IChange source )
		{
			return this.CreateMock<T>( entity, cachedValue, source, CommitReason.AcceptChanges );
		}

		protected ChangeCommittedEventArgs<T> CreateMock<T>( object entity, T cachedValue, IChange source, CommitReason reason )
		{
			return new ChangeCommittedEventArgs<T>( entity, cachedValue, source, reason );
		}

		[TestMethod]
		public void changeCommittedArgs_generic_ctor_normal_should_correctly_set_values()
		{
			var entity = new Object();
			var cachedValue = new GenericParameterHelper();
			var iChange = MockRepository.GenerateStub<IChange>();
			var reason = CommitReason.AcceptChanges;

			var target = this.CreateMock<GenericParameterHelper>( entity, cachedValue, iChange, reason );

			target.Entity.Should().Be.EqualTo( entity );
			target.CachedValue.Should().Be.EqualTo( cachedValue );
			target.Source.Should().Be.EqualTo( iChange );
			target.Reason.Should().Be.EqualTo( reason );
		}
	}
}
