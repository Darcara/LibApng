using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Omega.Lib.APNG.Chunks
	{
	public class Plte : APngChunk
		{
		public Plte() : base(ChunkType.PLTE)
			{}

		#region Overrides of APngChunk

		public override MemoryStream Data
			{
			get { throw new NotImplementedException(); }
			}

		#endregion
		}
	}
