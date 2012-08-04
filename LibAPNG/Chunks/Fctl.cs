using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Omega.Lib.APNG.Chunks
	{
	public class Fctl : APngChunk
		{
		public Fctl() : base(ChunkType.fcTL)
			{
			
			}

		#region Overrides of APngChunk

		public override MemoryStream Data
			{
			get { throw new NotImplementedException(); }
			}

		#endregion
		}
	}
