using System.IO;
using Omega.Lib.APNG.Chunks;

namespace Omega.Lib.APNG.Test
	{
	class Program
		{
		static void Main(string[] args)
			{
			var apng = new APNG(new Ihdr(1, 1, BitDepth._8, ColorType.Rgb));

			File.Delete("simple100x100.png");
			apng.ToFile("simple100x100.png");
			}
		}
	}
