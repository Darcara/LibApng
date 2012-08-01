using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omega.Lib.APNG.Exceptions
	{
	public class EncoderException : Exception
		{
		public EncoderException(String message)
			: base(message)
			{

			}
		public EncoderException(String message, Exception innerException)
			: base(message, innerException)
			{

			}
		}
	}
