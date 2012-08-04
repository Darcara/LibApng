using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Omega.Lib.APNG.Chunks
	{
	public class Idat : APngChunk
		{
		protected MemoryStream MemStream;

		public Idat(byte[] imageData, bool needToDeflate = false)
			: base(ChunkType.IDAT)
			{
			if(!needToDeflate)
				MemStream = new MemoryStream(imageData);
			else
				{
				MemStream = new MemoryStream();
				MemStream.WriteByte(0x78);
				MemStream.WriteByte(0x9c);
				using(var gzip = new DeflateStream(MemStream, CompressionMode.Compress, true))
					{
					gzip.Write(imageData, 0, imageData.Length);
					gzip.Flush();
					}
				MemStream.Write(Adler32(imageData, 0, imageData.Length), 0, 4);
				}
			}


		#region Overrides of APngChunk

		public override MemoryStream Data
			{
			get { return MemStream; }
			}

		#endregion


		private byte[] Adler32(byte[] stream, int offset,
													 int length)
			{
			var adler = 1;
			var len = length;
			var NMAX = 3854;
			var BASE = 65521;
			var s1 = adler & 0xffff;
			var s2 = ((adler & 0xffff0000) >> 16) & 0xFFFF;
			var k = 0;
			var bpos = offset;
			while(len > 0)
				{
				k = len < NMAX ? len : NMAX;
				len -= k;
				while(k > 0)
					{
					s1 = unchecked((int)s1 + stream[bpos]);
					s2 = unchecked((int)s2 + s1);
					bpos += 1;
					k -= 1;
					}
				s1 = s1 % BASE;
				s2 = s2 % BASE;
				}
			return new byte[]{(byte)(s2>>8),
				(byte)(s2&255),
				(byte)(s1>>8),
				(byte)(s1&255)
			};
			}
		}
	}
