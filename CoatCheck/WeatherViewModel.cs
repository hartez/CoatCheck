namespace CoatCheck
{
	public class WeatherViewModel
	{
		public WeatherViewModel(StationData stationData)
		{
			var current = stationData.current_conditions;

			Temp = current.air_temperature.ToDisplay();
			FeelsLike = current.feels_like.ToDisplay();

			CurrentConditions = current.conditions;
			CurrentIcon = current.icon;

			Hour1 = stationData.forecast.hourly[0];
			Hour2 = stationData.forecast.hourly[1];
			Hour3 = stationData.forecast.hourly[2];
		}

		public WeatherViewModel(double airTemp, double feelsLike)
		{	
			Temp = airTemp.ToDisplay();
			FeelsLike = feelsLike.ToDisplay();

			CurrentConditions = "Snow";
			CurrentIcon = "snow";

			Hour1 = new Hourly();
		}

		public string Temp { get; }
		public string FeelsLike { get; }

		public string CurrentConditions { get; }
		public string CurrentIcon { get; }

		public Hourly Hour1 { get; }
		public Hourly Hour2 { get; }
		public Hourly Hour3 { get; }
	}
}
