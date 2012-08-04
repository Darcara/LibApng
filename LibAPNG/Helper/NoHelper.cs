using System;

namespace Omega.Lib.APNG.Helper
	{
	public class NoHelper
		{
		public static byte[] GetBytes(UInt32 i) { return GetBytes((Int32)i); }

		public static byte[] GetBytes(Int32 i)
			{
			return new[]
			       	{
			       		(byte)((i >> 24) & 0xFF),
								(byte)((i >> 16) & 0xFF),
								(byte)((i >>  8) & 0xFF),
								(byte)((i >>  0) & 0xFF),
			       	};
			}
		}
	}
