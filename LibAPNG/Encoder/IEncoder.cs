using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Omega.Lib.APNG.Chunks;

namespace Omega.Lib.APNG.Encoder
	{
	public interface IEncoder
		{
		byte[] Encode(InternalImage img, InternalImage lastImage);
		Plte CreatePalette(InternalImage img);
		}
	}
