using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Omega.Lib.APNG.Chunks;
using Omega.Lib.APNG.Helper;

namespace Omega.Lib.APNG.Encoder
	{
	public interface IEncoder
		{
		Plte CreatePalette(InternalImage img);

		//byte[] Encode(InternalImage img, InternalImage lastImage);
		Tuple<Fctl, Idat> EncodeImage(Ihdr ihdr, InternalImage img, UInt32 sequenceNumber);
		Tuple<Fctl, Fdat> EncodeFrame(Ihdr ihdr, InternalImage img, UInt32 sequenceNumber, InternalImage lastImage, Rational delay);
		}
	}
