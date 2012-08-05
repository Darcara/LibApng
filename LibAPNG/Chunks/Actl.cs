using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Omega.Lib.APNG.Helper;

namespace Omega.Lib.APNG.Chunks
	{
	public class Actl : APngChunk
		{
		private readonly uint _numberOfFrames;
		private readonly uint _numberOfLoops;
		private readonly MemoryStream _memStream = new MemoryStream();

		public Actl(UInt32 numberOfFrames, UInt32 numberOfLoops) : base(ChunkType.acTL)
			{
			#region sanity
			if(numberOfFrames == 0)
				throw new ArgumentOutOfRangeException("numberOfFrames", "numberOfFrames cannot be 0");
			#endregion

			_numberOfFrames = numberOfFrames;
			_numberOfLoops = numberOfLoops;

			_memStream.Write(NoHelper.GetBytes(_numberOfFrames), 0, 4);
			_memStream.Write(NoHelper.GetBytes(_numberOfLoops), 0, 4);
			}

		#region Overrides of APngChunk

		public override MemoryStream Data
			{
			get { return _memStream; }
			}

		#endregion
		}
	}
