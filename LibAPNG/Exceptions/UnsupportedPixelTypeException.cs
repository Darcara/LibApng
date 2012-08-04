using System;

namespace Omega.Lib.APNG.Exceptions
	{
	public class UnsupportedPixelTypeException : Exception
		{
		public UnsupportedPixelTypeException(String message)
			: base(message)
		{ }
		public UnsupportedPixelTypeException(String message, Exception innerException)
			: base(message, innerException)
		{ }
		}
	}