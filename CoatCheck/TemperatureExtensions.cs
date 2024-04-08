using System;

namespace CoatCheck
{
	public static class TemperatureExtensions
	{
		public static string ToDisplay(this double temp)
		{
			return $"{Math.Round((temp * 9.0 / 5.0) + 32, 0)}°";
		}
	}
}
