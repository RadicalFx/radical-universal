﻿namespace RadicalTests.ChangeTracking
{
	using System;
	using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
	using Rhino.Mocks;
	

	[TestClass]
	public class AdvisoryTests
	{
		[TestMethod]
		[TestCategory( "ChangeTracking" )]
		public void advisory_ctor()
		{
			var expected = new IAdvisedAction[] 
			{
				MockRepository.GenerateStub<IAdvisedAction>(),
				MockRepository.GenerateStub<IAdvisedAction>(),
				MockRepository.GenerateStub<IAdvisedAction>()
			};

			var actual = new Advisory( expected );

			actual.Count.Should().Be.EqualTo( 3 );
			actual.Should().Have.SameSequenceAs( expected );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ) )]
		[TestCategory( "ChangeTracking" )]
		public void advisory_ctor_null_reference()
		{
			var actual = new Advisory( null );
		}
	}
}