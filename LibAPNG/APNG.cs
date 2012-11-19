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
		private static readonly byte[] _magicBytes = { 0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a };
		private UInt32 _sequenceNumber = 0;
		private InternalImage _lastImage = null;
		//private readonly List<APngChunk> _chunks = new List<APngChunk>();
		private readonly List<IDecoder> _decoders = new List<IDecoder>();

		protected Object DefaultImageObject = null;
		protected IEnumerable<Frame> FrameObjects = null;

		public IEncoder Encoder { get; set; }
		public Ihdr Ihdr { get; private set; }
		public byte[] Palette { get; protected set; }

		public Boolean DefaultImageSet { get { return DefaultImageObject != null; } }
		//public UInt32 FramesAdded { get; protected set; }
		public UInt32 Loops = 0;

		public APNG(Ihdr ihdr, UInt32 loops = 0)
			{
			Loops = loops;
			_crc32 = new Crc32();
			Ihdr = ihdr;
			}

		public void RegisterDecoder(IDecoder decoder)
			{
			if(!_decoders.Contains(decoder))
				_decoders.Add(decoder);
			}

		public void ToFile(String filename)
			{
			Stream fileOut = File.OpenWrite(filename);
			InternalImage img = null;

			try
				{
				foreach (var b in _magicBytes)
					fileOut.WriteByte(b);

				WriteChunk(Ihdr, fileOut);
				WriteChunk(new Actl((UInt32) FrameObjects.Count(), Loops), fileOut);

				//write pallette
				//if(img.RgbPaletteData != null)
				//  _chunks.Add(Encoder.CreatePalette(img));


				//write default image -->
				if(DefaultImageSet)
					{
					img = DecodeObject(DefaultImageObject);
					if (img == null)
						throw new InvalidDataException("No decoder for default image object type " + DefaultImageObject.GetType().Name);

					var image = Encoder.EncodeImage(Ihdr, img, _sequenceNumber - 1);
					WriteChunk(image.Item2, fileOut);
					}
				//<-- write default image

				//write frames -->
				foreach(var frameObject in FrameObjects)
					{
					img = DecodeObject(frameObject.FrameObject);
					if (img == null)
						throw new InvalidDataException("No decoder for frame(" + _sequenceNumber + ") object type " + frameObject.FrameObject.GetType().Name);

					if (!DefaultImageSet)
						{
						Tuple<Fctl, Idat> image = Encoder.EncodeImage(Ihdr, img, _sequenceNumber);

						WriteChunk(image.Item1, fileOut);
						WriteChunk(image.Item2, fileOut);
						_sequenceNumber += 1;
						}
					else
						{
						Tuple<Fctl, Fdat> frame = Encoder.EncodeFrame(Ihdr, img, _sequenceNumber, _lastImage, frameObject.Delay);

						WriteChunk(frame.Item1, fileOut);
						WriteChunk(frame.Item2, fileOut);
						_sequenceNumber += 2;
						}

					_lastImage = img;
					}
				//<-- write frames

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

		//public void AddKeyFrameFromObject(Object obj, Rational delay = default(Rational))
		//  {
		//  }
		public void SetFrames(IEnumerable<Frame> frames)
			{
			FrameObjects = frames;
			}

		/// <summary>
		/// Sets the default image that is not part of the movie
		/// </summary>
		/// <param name="obj"></param>
		public void AddDefaultImageFromObject(Object obj)
			{
			DefaultImageObject = obj;
			}

		protected InternalImage DecodeObject(Object obj)
			{
			IDecoder decoder = _decoders.FirstOrDefault(dec => dec.CanDecode(obj));
			return decoder == null ? null : decoder.Decode(obj, Ihdr, Palette);
			}
		}
	}
