using System;

namespace Omega.Lib.APNG.Helper
	{
	/// <summary>
	/// Stolen from http://www.libpng.org/pub/png/spec/1.2/PNG-CRCAppendix.html
	/// </summary>
	public sealed class Crc32
		{
		/* Table of CRCs of all 8-bit messages. */
		private static UInt64[] _crcTable = null;

		/* Make the table for a fast CRC. */
		private static void MakeCrcTable()
			{
			_crcTable = new UInt64[256];

			for(Int32 n = 0; n < 256; n++)
				{
				var c = (UInt64)n;
				Int32 k;
				for(k = 0; k < 8; k++)
					{
					if((c & 1) != 0)
						c = 0xedb88320L ^ (c >> 1);
					else
						c = c >> 1;
					}
				_crcTable[n] = c;
				}
			}

		/* Update a running CRC with the bytes buf[0..len-1]--the CRC
			 should be initialized to all 1's, and the transmitted value
			 is the 1's complement of the final running CRC (see the
			 crc() routine below)). */

		private static UInt64 UpdateCrc(UInt64 crc, Byte[] buf, Int32 len)
			{
			UInt64 c = crc;
			Int32 n;

			if(_crcTable == null)
				MakeCrcTable();
			for(n = 0; n < len; n++)
				{
				c = _crcTable[(c ^ buf[n]) & 0xff] ^ (c >> 8);
				}
			return c;
			}

		/* Return the CRC of the bytes buf[0..len-1]. */

		private static UInt64 Crc(Byte[] buf, Int32 len)
			{
			return UpdateCrc(0xffffffffL, buf, len) ^ 0xffffffffL;
			}

		private UInt64 _crc = 0xffffffffL;

		public void Reset()
			{
			_crc = 0xffffffffL;
			}

		public void Update(Byte[] buf, Int32 len)
			{
			_crc = UpdateCrc(_crc, buf, len);
			}

		public Int32 GetCrC()
			{
			return (Int32)(((_crc ^ 0xffffffffL) & 0xFFFFFFFF));
			}
		}
	}
