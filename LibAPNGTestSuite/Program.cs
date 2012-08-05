using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Omega.Lib.APNG.Chunks;
using Omega.Lib.APNG.Decoder;
using Omega.Lib.APNG.Encoder;
using Omega.Lib.APNG.Helper;

namespace Omega.Lib.APNG.Test
	{
	class Program
		{
		static void Main(string[] args)
			{
			uint dim = 100;

			var i = new Bitmap((int)dim, (int)dim, PixelFormat.Format24bppRgb);
			var g = Graphics.FromImage(i);
			g.FillRectangle(new SolidBrush(Color.Transparent), 0, 0, i.Width, i.Height);
			g.DrawEllipse(new Pen(Color.IndianRed), 1, 1, i.Width / 2, i.Height / 2);
			g.DrawRectangle(new Pen(Color.GreenYellow, 4), i.Width / 2, i.Width / 2, i.Width / 2 - 4, i.Width / 2 - 4);

			
			var ihdr = new Ihdr(dim, dim, BitDepth._8, ColorType.Rgb);
			var apng = new APNG(ihdr);

			apng.RegisterDecoder(new SimpleDecoder());
			apng.Encoder = new SimpleEncoder();

			apng.AddDefaultImageFromObject(i);

			for(int j = 0; j < i.Height / 2; j++)
				{
				g.Clear(Color.Blue);
				g.DrawEllipse(new Pen(Color.Lime), i.Width / 4, j, i.Width / 2, i.Height / 2);
				apng.AddKeyFrameFromObject(i, new Rational(10, 100));
				}
			
			if(File.Exists("simple100x100.png")) 
				File.Delete("simple100x100.png");
			apng.ToFile("simple100x100.png");
			}
		}
	}
