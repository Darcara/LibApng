using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omega.Lib.APNG.Chunks
	{
	public class ChunkType
		{
		public static readonly ChunkType IHDR = new ChunkType("IHDR");
		public static readonly ChunkType PLTE = new ChunkType("PLTE");
		public static readonly ChunkType IDAT = new ChunkType("IDAT");
		public static readonly ChunkType IEND = new ChunkType("IEND");
		public static readonly ChunkType cHRM = new ChunkType("cHRM");// No Before PLTE and IDAT
		public static readonly ChunkType gAMA = new ChunkType("gAMA");// No Before PLTE and IDAT
		public static readonly ChunkType iCCP = new ChunkType("iCCP");// No Before PLTE and IDAT
		public static readonly ChunkType sBIT = new ChunkType("sBIT");// No Before PLTE and IDAT
		public static readonly ChunkType sRGB = new ChunkType("sRGB");// No Before PLTE and IDAT
		public static readonly ChunkType bKGD = new ChunkType("bKGD");// No After PLTE; before IDAT
		public static readonly ChunkType hIST = new ChunkType("hIST");// No After PLTE; before IDAT
		public static readonly ChunkType tRNS = new ChunkType("tRNS");// No After PLTE; before IDAT
		public static readonly ChunkType pHYs = new ChunkType("pHYs");// No Before IDAT
		public static readonly ChunkType sPLT = new ChunkType("sPLT");// Yes Before IDAT
		public static readonly ChunkType tIME = new ChunkType("tIME");// No None
		public static readonly ChunkType iTXt = new ChunkType("iTXt");// Yes None
		public static readonly ChunkType tEXt = new ChunkType("tEXt");// Yes None
		public static readonly ChunkType zTXt = new ChunkType("zTXt");// Yes None

		public String Name { get; protected set; }
		public Byte[] Bytes { get { return Encoding.UTF7.GetBytes(Name); } }


		public ChunkType(String identifier)
			{
			Name = identifier;
			}

		public ChunkType(String name, Boolean ancillary, Boolean privat, Boolean reserved, Boolean safeToCopy)
			{
			throw new NotImplementedException();
			}
		}
	}
