using System;

namespace CoatCheck
{
	public static class FormatExtensions
	{
		public static string ToDisplay(this double temp)
		{
			return $"{Math.Round((temp * 9.0 / 5.0) + 32, 0)}°";
		}

		public static string ToHourDisplay(this Hourly hourly)
		{
			var span = TimeSpan.FromHours(hourly.local_hour);
            return (DateTime.Today.Add(span)).ToString("h tt");
		}

		public static string ToShortCondition(this string condition)
		{
			return condition.Replace(" Possible", "?");
		}
	}
}
