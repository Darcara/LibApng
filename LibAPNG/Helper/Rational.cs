using System;

namespace Omega.Lib.APNG.Helper
	{
	public struct Rational
		{
		private const Double _epsilon = .000001d;

		public UInt16 Numerator;
		public UInt16 Denominator;

		public Rational(UInt16 num, UInt16 den)
			{
			Numerator = num;
			Denominator = den;
			}

		public Rational(Double value)
			{


			UInt16 numerator = 1;
			UInt16 denominator = 1;
			Double fraction = 1.0;

			while(Math.Abs(fraction - value) > _epsilon)
				{
				if(fraction < value)
					numerator++;
				else
					{
					denominator++;
					numerator = (UInt16)Math.Round(value * denominator);
					}

				fraction = numerator / (Double)denominator;
				}

			Numerator = numerator;
			Denominator = denominator;
			}
		}
	}
