using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Omega.Lib.APNG.Chunks
	{
	public abstract class APngChunk
		{
		public ChunkType ChunkType { get; private set; }
		public abstract MemoryStream Data { get; }

		protected APngChunk(ChunkType type)
			{
			ChunkType = type;
			}
		}
	}
