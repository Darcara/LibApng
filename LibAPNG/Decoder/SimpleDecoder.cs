using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Omega.Lib.APNG.Chunks;

namespace Omega.Lib.APNG.Decoder
	{
	public class SimpleDecoder : IDecoder
		{

		#region RGB
		private byte[] CreateImageRgb(Bitmap bitmap, Ihdr ihdr)
			{
			BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb); //will actually be bgr
			try
				{
				var row = new byte[bitmap.Width * 3];
				var destBytes = new byte[bitmap.Width * 3 * bitmap.Height];
				for(int y = 0; y < bmpData.Height; ++y)
					{
					Marshal.Copy(bmpData.Scan0 + (y * bmpData.Stride), row, 0, bmpData.Width * 3);
					for(int i = 0; i < bitmap.Width * 3; i+=3)
						{
						destBytes[y * ((bmpData.Width * 3)) + i] = row[i + 2];
						destBytes[y * ((bmpData.Width * 3)) + i + 1] = row[i + 1];
						destBytes[y * ((bmpData.Width * 3)) + i + 2] = row[i];
						}
					}

				//var destBytes = new byte[bitmap.Width * 3 * bitmap.Height + 1];
				//destBytes[0] = 0x00;
				//for(int y = 0; y < bmpData.Height; ++y)
				//  Marshal.Copy(bmpData.Scan0 + y * bmpData.Stride, destBytes, 1 + (y * bitmap.Width * 3), bmpData.Width * 3);

				return destBytes;
				}
			finally
				{
				bitmap.UnlockBits(bmpData);
				}
			}
		#endregion

		#region palette image
		private byte[] CreatePalette(Bitmap bitmap)
			{
			throw new NotImplementedException();
			}
		private byte[] CreateImageIndices(Bitmap bitmap, Ihdr ihdr, byte[] palette)
			{
			throw new NotImplementedException();
			}
		#endregion


		#region Implementation of IDecoder<Bitmap>

		public bool CanDecode(object obj)
			{
			return obj is Bitmap;
			}

		public InternalImage Decode(Object obj, Ihdr ihdr, byte[] usePalette = null)
			{
			if(obj == null)
				throw new ArgumentNullException("obj");
			var bitmap = obj as Bitmap;
			if(bitmap == null)
				throw new NotSupportedException(GetType().Name + " is unable to decode object type " + obj.GetType().Name);

			#region sanity
			if(bitmap.Width != ihdr.Width)
				throw new ArgumentException("bitmap.Width should be " + ihdr.Width + " is " + bitmap.Width, "obj");
			if(bitmap.Height != ihdr.Height)
				throw new ArgumentException("bitmap.Height should be " + ihdr.Height + " is " + bitmap.Height, "obj");
			#endregion

			var intImage = new InternalImage(ihdr);


			switch (ihdr.ColorType)
				{
				case ColorType.Palette:
					if(usePalette == null)
						intImage.RgbPaletteData = CreatePalette(bitmap);
					intImage.RgbData = CreateImageIndices(bitmap, ihdr, usePalette ?? intImage.RgbPaletteData);
					break;
				case ColorType.Grayscale:
					throw new NotImplementedException();
					break;
				case ColorType.Rgb:
					intImage.RgbData =CreateImageRgb(bitmap, ihdr);
					break;
				case ColorType.GrayscaleWithAlpha:
					throw new NotImplementedException();
					break;
				case ColorType.Rgba:
					throw new NotImplementedException();
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}


			#region unable to process

			//switch(bitmap.PixelFormat)
			//  {
			//  case PixelFormat.Indexed:
			//    throw new NotImplementedException();
			//    break;
			//  case PixelFormat.Format1bppIndexed:
			//    throw new NotImplementedException();
			//    break;
			//  case PixelFormat.Format4bppIndexed:
			//    throw new NotImplementedException();
			//    break;
			//  case PixelFormat.Format8bppIndexed:
			//    throw new NotImplementedException();
			//    break;
			//  case PixelFormat.Format16bppGrayScale:
			//    throw new NotImplementedException();
			//    break;
			//  case PixelFormat.Format16bppRgb555:
			//    throw new NotImplementedException();
			//    break;
			//  case PixelFormat.Format16bppRgb565:
			//    throw new NotImplementedException();
			//    break;
			//  case PixelFormat.Format16bppArgb1555:
			//    throw new NotImplementedException();
			//    break;
			//  case PixelFormat.Format24bppRgb:
			//    throw new NotImplementedException();
			//    break;
			//  case PixelFormat.Format32bppRgb:
			//    if(ihdr.ColorType == ColorType.Rgb && ihdr.BitDepth == BitDepth._8)
			//      ;
			//    else
			//      throw new NotImplementedException();
			//    break;
			//  case PixelFormat.Format32bppArgb:
			//    throw new NotImplementedException();
			//    break;
			//  //case PixelFormat.Format32bppPArgb:
			//  //case PixelFormat.Alpha:
			//  //case PixelFormat.PAlpha:
			//  //case PixelFormat.Extended:
			//  //case PixelFormat.Canonical:
			//  //case PixelFormat.Undefined:
			//  //case PixelFormat.Format48bppRgb:
			//  //case PixelFormat.Format64bppArgb:
			//  //case PixelFormat.Format64bppPArgb:
			//  //case PixelFormat.Max:
			//  //case PixelFormat.Gdi:
			//  default:
			//    throw new UnsupportedPixelTypeException("Image pixel format unsupported: " + bitmap.PixelFormat);
			//  }

			#endregion

			return intImage;
			}

		#endregion
		}
	}
