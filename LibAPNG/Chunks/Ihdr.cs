using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Omega.Lib.APNG.Chunks
	{
	public class Ihdr : APngChunk
		{
		private readonly uint _width;
		private readonly uint _height;
		private readonly BitDepth _bitDepth;
		private readonly ColorType _colorType;
		private readonly CompressionMethod _compressionMethod;
		private readonly FilterMethod _filterMethod;
		private readonly InterlaceMethod _interlaceMethod;

		public Ihdr(UInt32 width, UInt32 height, BitDepth bitDepth, ColorType colorType, CompressionMethod compressionMethod = CompressionMethod.Default, FilterMethod filterMethod = FilterMethod.Default, InterlaceMethod interlaceMethod = InterlaceMethod.None) : base(ChunkType.IHDR)
			{
			#region Sanity
			if(width == 0 || width > Int32.MaxValue)
				throw new ArgumentOutOfRangeException("width", "width must be greater than 0 and smaller than In32.MaxValue(2^31-1)");
			if(height == 0 || height > Int32.MaxValue)
				throw new ArgumentOutOfRangeException("height", "height must be greater than 0 and smaller than In32.MaxValue(2^31-1)");

			BitDepth[] allowedBitDepths;
			switch (colorType)
				{
				case ColorType.Grayscale:
					if(!(allowedBitDepths = new[] { BitDepth._1, BitDepth._2, BitDepth._4, BitDepth._8, BitDepth._16 }).Contains(bitDepth))
						throw new ArgumentOutOfRangeException("bitDepth", String.Format("bitDepth must be one of {0} for colorType {1}", allowedBitDepths.Aggregate("", (s, bd) => s + "bd, ", s => s.Trim().Substring(0, s.Length - 1)), colorType));
					break;
				case ColorType.Rgb:
					if(!(allowedBitDepths = new[]{BitDepth._8, BitDepth._16}).Contains(bitDepth))
						throw new ArgumentOutOfRangeException("bitDepth", String.Format("bitDepth must be one of {0} for colorType {1}", allowedBitDepths.Aggregate("", (s, bd) => s + "bd, ", s => s.Trim().Substring(0, s.Length-1)), colorType));
					break;
				case ColorType.Palette:
					if(!(allowedBitDepths = new[] { BitDepth._1, BitDepth._2, BitDepth._4, BitDepth._8}).Contains(bitDepth))
						throw new ArgumentOutOfRangeException("bitDepth", String.Format("bitDepth must be one of {0} for colorType {1}", allowedBitDepths.Aggregate("", (s, bd) => s + "bd, ", s => s.Trim().Substring(0, s.Length - 1)), colorType));
					break;
				case ColorType.GrayscaleWithAlpha:
					if(!(allowedBitDepths = new[] { BitDepth._8, BitDepth._16}).Contains(bitDepth))
						throw new ArgumentOutOfRangeException("bitDepth", String.Format("bitDepth must be one of {0} for colorType {1}", allowedBitDepths.Aggregate("", (s, bd) => s + "bd, ", s => s.Trim().Substring(0, s.Length - 1)), colorType));
					break;
				case ColorType.Rgba:
					if(!(allowedBitDepths = new[] { BitDepth._8, BitDepth._16}).Contains(bitDepth))
						throw new ArgumentOutOfRangeException("bitDepth", String.Format("bitDepth must be one of {0} for colorType {1}", allowedBitDepths.Aggregate("", (s, bd) => s + "bd, ", s => s.Trim().Substring(0, s.Length - 1)), colorType));
					break;
				default:
					throw new ArgumentOutOfRangeException("colorType", String.Format("Unknown colorType: {0}", colorType));
				}

			if(compressionMethod != CompressionMethod.Default)
				throw new ArgumentOutOfRangeException("compressionMethod", String.Format("Unknown compressionMethod: {0}", compressionMethod));
			if(filterMethod != FilterMethod.Default)
				throw new ArgumentOutOfRangeException("filterMethod", String.Format("Unknown filterMethod: {0}", filterMethod));

			var allowedInterlaceMethods = new[] {InterlaceMethod.None, InterlaceMethod.Adam7};
			if(!allowedInterlaceMethods.Contains(interlaceMethod))
				throw new ArgumentOutOfRangeException("interlaceMethod", String.Format("interlaceMethod must be one of {0}", allowedInterlaceMethods.Aggregate("", (s, bd) => s + "bd, ", s => s.Trim().Substring(0, s.Length - 1))));

			#endregion

			_width = width;
			_height = height;
			_bitDepth = bitDepth;
			_colorType = colorType;
			_compressionMethod = compressionMethod;
			_filterMethod = filterMethod;
			_interlaceMethod = interlaceMethod;
			}

		#region Implementation of APngChunk

		public override MemoryStream Data
			{
			get
				{
				var ms = new MemoryStream();

				var writer = new BinaryWriter(ms);
				writer.Write(BitConverter.GetBytes(_width).Reverse().ToArray());
				writer.Write(BitConverter.GetBytes(_height).Reverse().ToArray());
				writer.Write((Byte)_bitDepth);
				writer.Write((Byte)_colorType);
				writer.Write((Byte)_compressionMethod);
				writer.Write((Byte)_filterMethod);
				writer.Write((Byte)_interlaceMethod);
				

				return ms;
				}
			}

		#endregion
		}
	}
