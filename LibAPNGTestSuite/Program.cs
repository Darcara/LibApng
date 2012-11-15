using System;
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
			TestEncoders();
			}

		private static void TestEncoders()
			{
			const UInt32 imageDimension = 100;

			TestEncoder(imageDimension, new SimpleEncoder());
			TestEncoder(imageDimension, new DefaultEncoder());
			}

		private static void TestEncoder(UInt32 imageDimension, IEncoder enc)
			{
			var i = new Bitmap((int)imageDimension, (int)imageDimension, PixelFormat.Format24bppRgb);
			var g = Graphics.FromImage(i);
			g.FillRectangle(new SolidBrush(Color.Transparent), 0, 0, i.Width, i.Height);
			g.DrawEllipse(new Pen(Color.IndianRed), 1, 1, i.Width / 2, i.Height / 2);
			g.DrawRectangle(new Pen(Color.GreenYellow, 4), i.Width / 2, i.Width / 2, i.Width / 2 - 4, i.Width / 2 - 4);


			var ihdr = new Ihdr(imageDimension, imageDimension, BitDepth._8, ColorType.Rgb);
			var apng = new APNG(ihdr);

			apng.RegisterDecoder(new SimpleDecoder());
			apng.Encoder = enc;
			apng.AddDefaultImageFromObject(i);

			for(int j = 0; j < i.Height / 2; j++)
				{
				g.Clear(Color.Blue);
				g.DrawEllipse(new Pen(Color.Lime), i.Width / 4, j, i.Width / 2, i.Height / 2);
				apng.AddKeyFrameFromObject(i, new Rational(10, 100));
				}

			String fileName = String.Format("{0}_{1}x{2}_{3}.png", enc.GetType().Name, imageDimension, imageDimension, 0);

			if(File.Exists(fileName))
				File.Delete(fileName);
			apng.ToFile(fileName);





			ihdr = new Ihdr(imageDimension, imageDimension, BitDepth._8, ColorType.Rgb);
			apng = new APNG(ihdr);
			apng.RegisterDecoder(new SimpleDecoder());
			apng.Encoder = enc;

			String nowString = DateTime.UtcNow.ToString(@"yyyy\-MM\-dd HH:mm");
			g.Clear(Color.Blue);
			g.DrawString(nowString, new Font("Consolas", 7), new SolidBrush(Color.Lime), 5, i.Height/2f);
			apng.AddDefaultImageFromObject(i);

			for(int j = 10; j < i.Height / 2; j++)
				{
				g.Clear(Color.Blue);
				g.DrawString(nowString, new Font("Consolas", 7), new SolidBrush(Color.Lime), 5, j);
				apng.AddKeyFrameFromObject(i, new Rational(10, 100));
				}

			fileName = String.Format("{0}_{1}x{2}_{3}.png", enc.GetType().Name, imageDimension, imageDimension, 1);

			if(File.Exists(fileName))
				File.Delete(fileName);
			apng.ToFile(fileName);
			}
		}
	}
