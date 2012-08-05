using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Omega.Lib.APNG.Helper;

namespace Omega.Lib.APNG.Chunks
	{
	public class Fctl : APngChunk
		{
		private readonly UInt32 _frame;
		private readonly UInt32 _x;
		private readonly UInt32 _y;
		private readonly UInt32 _width;
		private readonly UInt32 _height;
		private readonly Rational _delay;
		private readonly ApngDisposeOperation _disposeOp;
		private readonly ApngBlendOperation _blendOp;

		private readonly MemoryStream _memStream = new MemoryStream();

		public Fctl(Ihdr ihdr, UInt32 frame, UInt32 x, UInt32 y, UInt32 width, UInt32 height, Rational delay, ApngDisposeOperation disposeOp, ApngBlendOperation blendOp)
			: base(ChunkType.fcTL)
			{
			#region sanity
			if(width == 0)
				throw new ArgumentOutOfRangeException("width", "width must be > 0");
			if(height == 0)
				throw new ArgumentOutOfRangeException("height", "height must be > 0");

			if(x + width > ihdr.Width)
				throw new ArgumentException("x + width(" + (x + (UInt64)width) + ") must be smaller than canvas width: " + ihdr.Width);
			if(y + height > ihdr.Height)
				throw new ArgumentException("y + height(" + (y + (UInt64)height) + ") must be smaller than canvas height: " + ihdr.Height);
			#endregion

			_frame = frame;
			_x = x;
			_y = y;
			_width = width;
			_height = height;
			_delay = delay;
			_disposeOp = disposeOp;
			_blendOp = blendOp;

			_memStream.Write(NoHelper.GetBytes(_frame), 0, 4);
			_memStream.Write(NoHelper.GetBytes(_width), 0, 4);
			_memStream.Write(NoHelper.GetBytes(_height), 0, 4);
			_memStream.Write(NoHelper.GetBytes(_x), 0, 4);
			_memStream.Write(NoHelper.GetBytes(_y), 0, 4);
			_memStream.Write(NoHelper.GetBytes(delay.Numerator), 0, 2);
			_memStream.Write(NoHelper.GetBytes(delay.Denominator), 0, 2);
			_memStream.WriteByte((Byte)disposeOp);
			_memStream.WriteByte((Byte)blendOp);
			}

		#region Overrides of APngChunk

		public override MemoryStream Data
			{
			get { return _memStream; }
			}

		#endregion
		}
	}
