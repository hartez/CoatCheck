namespace CoatCheck
{
	public class WeatherViewModel
	{
		public WeatherViewModel(StationData stationData)
		{
			var ob = stationData.obs[0];

			Temp = ob.air_temperature.ToDisplay();
			FeelsLike = ob.feels_like.ToDisplay();
		}

		public WeatherViewModel(double airTemp, double feelsLike)
		{	
			Temp = airTemp.ToDisplay();
			FeelsLike = feelsLike.ToDisplay();
		}

		public string Temp { get; }
		public string FeelsLike { get; }
	}
}
