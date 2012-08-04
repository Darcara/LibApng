using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using MS.Internal.Xml.XPath;
using Omega.Lib.APNG.Chunks;
using Omega.Lib.APNG.Decoder;
using Omega.Lib.APNG.Encoder;
using Omega.Lib.APNG.Exceptions;
using Omega.Lib.APNG.Helper;

namespace Omega.Lib.APNG
	{
	/// <summary>
	/// This is an animated portable network graphic 
	/// </summary>
	public class APNG
		{
		[ThreadStatic]
		private static Crc32 _crc32;
		public static readonly byte[] MagicBytes = { 0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a };

		private readonly List<APngChunk> _chunks = new List<APngChunk>();
		private readonly List<IDecoder> _decoders = new List<IDecoder>();

		public IEncoder Encoder { get; set; }
		public Ihdr Ihdr { get; private set; }
		public byte[] Palette { get; protected set; }

		public Boolean DefaultImageSet { get; protected set; }
		public UInt32 FramesAdded { get; protected set; }
		
		public APNG(Ihdr ihdr)
			{
			_crc32 = new Crc32();
			Ihdr = ihdr;
			_chunks.Add(ihdr);
			}

		public void RegisterDecoder(IDecoder decoder)
			{
			if(!_decoders.Contains(decoder))
				_decoders.Add(decoder);
			}


		public void ToFile(String filename)
			{
			Stream fileOut = File.OpenWrite(filename);
			try
				{
				foreach (var b in MagicBytes)
					fileOut.WriteByte(b);

				foreach(var pngChunk in _chunks)
					WriteChunk(pngChunk, fileOut);

				WriteChunk(new Iend(), fileOut);
				}
			finally
				{
				fileOut.Flush();
				fileOut.Close();
				}
			}

		protected Int64 WriteChunk(APngChunk chunk, Stream stream)
			{
			MemoryStream data = chunk.Data;
			Int64 dataLength = data.Length;

			if(data.Length > Int32.MaxValue)
				throw new EncoderException("Chunk data lengh cannot exceed Int32.MaxValue(2^31-1)");

			WriteBytes(stream, NoHelper.GetBytes((UInt32)dataLength), 4);
			_crc32.Reset();
			WriteBytes(stream, chunk.ChunkType.Bytes, 4);
			WriteBytes(stream, data.ToArray(), (Int32)dataLength);
			stream.Write(NoHelper.GetBytes(_crc32.GetCrC()), 0, 4);

			return dataLength;
			}

		protected void WriteBytes(Stream stream, byte[] bytes, Int32 count)
			{
			if(count == 0 || bytes == null)
				return;

			_crc32.Update(bytes, count);
			stream.Write(bytes, 0, count);
			}

		public void AddFrameFromObject(Object obj)
			{
			throw new NotImplementedException();
			}

		/// <summary>
		/// Sets the default image that is not part of the movie
		/// </summary>
		/// <param name="obj"></param>
		public void AddDefaultImageFromObject(Object obj)
			{
			if(obj == null)
				throw new ArgumentNullException("obj");

			if(DefaultImageSet)
				throw new ArgumentException("Default image already set");

			InternalImage img = DecodeObject(obj);
			if(img == null)
				throw  new ArgumentException("No decoder for object type " + obj.GetType().Name);

			if(img.RgbPaletteData != null)
				_chunks.Add(Encoder.CreatePalette(img));

			_chunks.Add(new Idat(Encoder.Encode(img), true));

			DefaultImageSet = true;
			}

		public InternalImage DecodeObject(Object obj)
			{
			IDecoder decoder = _decoders.FirstOrDefault(dec => dec.CanDecode(obj));
			return decoder == null ? null : decoder.Decode(obj, Ihdr, Palette);
			}
		}
	}
