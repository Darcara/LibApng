﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Omega.Lib.APNG.Chunks;
using Omega.Lib.APNG.Helper;

namespace Omega.Lib.APNG.Encoder
	{
	public class SimpleEncoder : IEncoder
		{
		protected byte[] Encode(InternalImage img, InternalImage lastImg)
			{
			Int64 stride = img.RgbData.Length / img.Ihdr.Height;
			var pngData = new byte[img.Ihdr.Height * (stride + 1)];

			for(Int64 y = 0; y < img.Ihdr.Height; ++y)
				{
				Int64 rowStart = y * (stride + 1);
				pngData[rowStart] = 0x00;	//Filter None
				Array.Copy(img.RgbData, y * stride, pngData, rowStart + 1, stride);
				}

			return pngData;
			}
		
		#region Implementation of IEncoder

		public Plte CreatePalette(InternalImage img)
			{
			throw new NotImplementedException();
			}

		public Tuple<Fctl, Idat> EncodeImage(Ihdr ihdr, InternalImage img, uint sequenceNumber)
			{
			return new Tuple<Fctl, Idat>(new Fctl(ihdr, sequenceNumber, 0, 0, img.Ihdr.Width, img.Ihdr.Height, new Rational(0, 0), ApngDisposeOperation.None, ApngBlendOperation.Source), new Idat(Encode(img, null), true));
			}

		public Tuple<Fctl, Fdat> EncodeFrame(Ihdr ihdr, InternalImage img, uint sequenceNumber, InternalImage lastImage, Rational delay)
			{
			return new Tuple<Fctl, Fdat>(new Fctl(ihdr, sequenceNumber, 0, 0, img.Ihdr.Width, img.Ihdr.Height, delay, ApngDisposeOperation.None, ApngBlendOperation.Source), new Fdat(sequenceNumber+1, Encode(img, lastImage), true));
			}

		#endregion
		}
	}
