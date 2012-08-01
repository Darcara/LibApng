using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Omega.Lib.APNG.Chunks;
using Omega.Lib.APNG.Exceptions;
using Omega.Lib.APNG.Hash;

namespace Omega.Lib.APNG
	{
	/// <summary>
	/// This is an animated portable network graphic 
	/// </summary>
	public class APNG
		{
		public static readonly byte[] MagicBytes = { 0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a };
		[ThreadStatic]
		private static Crc32 _crc32;

		protected List<APngChunk> Chunks = new List<APngChunk>();

		public APNG(Ihdr ihdr)
			{
			_crc32 = new Crc32();
			AddChunk(ihdr);
			}

		public void AddChunk(APngChunk chunk)
			{
			Chunks.Add(chunk);
			}

		public void ToFile(String filename)
			{
			Stream fileOut = File.OpenWrite(filename);
			try
				{
				foreach (var b in MagicBytes)
					fileOut.WriteByte(b);

				foreach(var pngChunk in Chunks)
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

			_crc32.Reset();
			WriteBytes(stream, BitConverter.GetBytes((UInt32) dataLength).Reverse().ToArray(), 0, 4);
			WriteBytes(stream, chunk.ChunkType.Bytes, 0, 4);
			WriteBytes(stream, data.GetBuffer(), 0, (Int32)dataLength);
			
			stream.Write(_crc32.Finish().Reverse().ToArray(), 0, 4);

			return dataLength;
			}

		protected void WriteBytes(Stream stream, byte[] bytes, Int32 offset, Int32 count)
			{
			_crc32.Update(bytes, offset, count);
			stream.Write(bytes, offset, count);
			}
		}
	}
