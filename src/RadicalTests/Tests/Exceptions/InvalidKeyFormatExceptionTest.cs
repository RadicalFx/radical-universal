﻿using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;


namespace RadicalTests.Exceptions
{
	[TestClass()]
	public class InvalidKeyFormatExceptionExceptionTest : RadicalExceptionTest
	{
		protected override Exception CreateMock()
		{
			return new InvalidKeyFormatException();
		}

		protected override Exception CreateMock( String message )
		{
			return new InvalidKeyFormatException( message );
		}

		protected override Exception CreateMock( String message, Exception innerException )
		{
			return new InvalidKeyFormatException( message, innerException );
		}
	}
}
