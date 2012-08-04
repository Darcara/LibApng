using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Omega.Lib.APNG.Chunks;

namespace Omega.Lib.APNG.Decoder
	{
	public interface IDecoder
		{
		Boolean CanDecode(Object obj);
		InternalImage Decode(Object obj, Ihdr ihdr, byte[] usePalette = null);
		}
	}
