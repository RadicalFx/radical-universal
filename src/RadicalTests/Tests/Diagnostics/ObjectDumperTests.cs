using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Topics.Radical.Diagnostics;


namespace RadicalTests.Diagnostics
{
	[TestClass]
	public class ObjectDumperTests
	{
		[TestMethod]
		[TestCategory( "ObjectDumper" )]
		public void ObjectDumper_Dump_using_valid_exception_with_innerException_should_behave_as_expected()
		{
			try
			{
				var e = new Exception( "Outer Exception", new Exception( "Inner Exception" ) );
				throw e;
			}
			catch( Exception error ) 
			{
				var dump = ObjectDumper.Dump( error );
				dump.Should().Not.Be.NullOrEmpty();
			}
		}
	}
}
