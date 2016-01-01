﻿namespace RadicalTests.ChangeTracking
{
	using System;
	using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
	using Rhino.Mocks;
	using Topics.Radical.ChangeTracking;
	using Topics.Radical.ComponentModel.ChangeTracking;
	

	[TestClass]
	public class ChangeSetTests
	{
		[TestMethod]
		[TestCategory( "ChangeTracking" )]
		public void changeSet_ctor()
		{
			var expected = new IChange[] 
			{
				MockRepository.GenerateStub<IChange>(),
				MockRepository.GenerateStub<IChange>(),
				MockRepository.GenerateStub<IChange>()
			};

			var actual = new ChangeSet( expected );

			actual.Count.Should().Be.EqualTo( 3 );
			actual.Should().Have.SameSequenceAs( expected );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ) )]
		[TestCategory( "ChangeTracking" )]
		public void changeSet_ctor_null_reference()
		{
			var actual = new ChangeSet( null );
		}
	}
}