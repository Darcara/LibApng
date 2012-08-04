using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Omega.Lib.APNG.Chunks;

namespace Omega.Lib.APNG.Encoder
	{
	public class SimpleEncoder : IEncoder
		{
		public SimpleEncoder()
			{
			
			}

		#region Implementation of IEncoder

		public byte[] Encode(InternalImage img)
			{
			Int64 stride = img.RgbData.Length/img.Ihdr.Height;
			var pngData = new byte[img.Ihdr.Height * (stride + 1)];

			for(Int64 y = 0; y < img.Ihdr.Height; ++y)
				{
				Int64 rowStart = y*(stride + 1);
				pngData[rowStart] = 0x00;	//Filter None
				Array.Copy(img.RgbData, y * stride, pngData, rowStart + 1, stride);
				}

			return pngData;
			}

		public Plte CreatePalette(InternalImage img)
			{
			throw new NotImplementedException();
			}

		#endregion
		}
	}
