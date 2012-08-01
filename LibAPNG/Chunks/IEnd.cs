using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Omega.Lib.APNG.Chunks
	{
	public class Iend : APngChunk
		{
		public Iend() : base(ChunkType.IEND)
		{ }


		#region Overrides of APngChunk

		public override MemoryStream Data
			{
			get { return new MemoryStream(); }
			}

		#endregion
		}
	}
