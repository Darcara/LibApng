using Omega.Lib.APNG.Chunks;

namespace Omega.Lib.APNG
	{
	public class InternalImage
		{
		public readonly Ihdr Ihdr;
		public byte[] RgbData;
		public byte[] RgbPaletteData;

		public InternalImage(Ihdr ihdr)
			{
			Ihdr = ihdr;
			}
		public InternalImage(Ihdr ihdr, byte[] rgbData, byte[] rgbPaletteData)
			{
			Ihdr = ihdr;
			RgbData = rgbData;
			RgbPaletteData = rgbPaletteData;
			}
		}
	}
