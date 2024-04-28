using System.Collections.Generic;
using System.Text.Json;

namespace CoatCheck
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "No point in wasting cycles on a JSON naming convention for this request, the data is clear enough")]
	public class CurrentConditions
	{
		public double air_density { get; set; }
		public double air_temperature { get; set; }
		public int brightness { get; set; }
		public string conditions { get; set; }
		public double delta_t { get; set; }
		public double dew_point { get; set; }
		public double feels_like { get; set; }
		public string icon { get; set; }
		public bool is_precip_local_day_rain_check { get; set; }
		public bool is_precip_local_yesterday_rain_check { get; set; }
		public int lightning_strike_count_last_1hr { get; set; }
		public int lightning_strike_count_last_3hr { get; set; }
		public int lightning_strike_last_distance { get; set; }
		public string lightning_strike_last_distance_msg { get; set; }
		public int lightning_strike_last_epoch { get; set; }
		public double precip_accum_local_day { get; set; }
		public double precip_accum_local_yesterday { get; set; }
		public int precip_minutes_local_day { get; set; }
		public int precip_minutes_local_yesterday { get; set; }
		public string pressure_trend { get; set; }
		public int relative_humidity { get; set; }
		public double sea_level_pressure { get; set; }
		public int solar_radiation { get; set; }
		public double station_pressure { get; set; }
		public int time { get; set; }
		public int uv { get; set; }
		public double wet_bulb_globe_temperature { get; set; }
		public double wet_bulb_temperature { get; set; }
		public double wind_avg { get; set; }
		public int wind_direction { get; set; }
		public string wind_direction_cardinal { get; set; }
		public double wind_gust { get; set; }
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "No point in wasting cycles on a JSON naming convention for this request, the data is clear enough")]
	public class Daily
	{
		public double air_temp_high { get; set; }
		public double air_temp_low { get; set; }
		public string conditions { get; set; }
		public int day_num { get; set; }
		public int day_start_local { get; set; }
		public string icon { get; set; }
		public int month_num { get; set; }
		public string precip_icon { get; set; }
		public int precip_probability { get; set; }
		public string precip_type { get; set; }
		public int sunrise { get; set; }
		public int sunset { get; set; }
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "No point in wasting cycles on a JSON naming convention for this request, the data is clear enough")]
	public class Forecast
	{
		public List<Daily> daily { get; set; }
		public List<Hourly> hourly { get; set; }
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "No point in wasting cycles on a JSON naming convention for this request, the data is clear enough")]
	public class Hourly
	{
		public double air_temperature { get; set; }
		public string conditions { get; set; }
		public double feels_like { get; set; }
		public string icon { get; set; }
		public int local_day { get; set; }
		public int local_hour { get; set; }
		public double precip { get; set; }
		public string precip_icon { get; set; }
		public int precip_probability { get; set; }
		public string precip_type { get; set; }
		public int relative_humidity { get; set; }
		public double sea_level_pressure { get; set; }
		public int time { get; set; }
		public double uv { get; set; }
		public double wind_avg { get; set; }
		public int wind_direction { get; set; }
		public string wind_direction_cardinal { get; set; }
		public double wind_gust { get; set; }
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "No point in wasting cycles on a JSON naming convention for this request, the data is clear enough")]
	public class StationData
	{
		public CurrentConditions current_conditions { get; set; }
		public Forecast forecast { get; set; }
		public double latitude { get; set; }
		public string location_name { get; set; }
		public double longitude { get; set; }
		public int source_id_conditions { get; set; }
		public Status status { get; set; }
		public string timezone { get; set; }
		public int timezone_offset_minutes { get; set; }
		public Units units { get; set; }

		public static StationData Parse(string data)
		{
			return JsonSerializer.Deserialize<StationData>(data);
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "No point in wasting cycles on a JSON naming convention for this request, the data is clear enough")]
	public class Status
	{
		public int status_code { get; set; }
		public string status_message { get; set; }
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "No point in wasting cycles on a JSON naming convention for this request, the data is clear enough")]
	public class Units
	{
		public string units_air_density { get; set; }
		public string units_brightness { get; set; }
		public string units_distance { get; set; }
		public string units_other { get; set; }
		public string units_precip { get; set; }
		public string units_pressure { get; set; }
		public string units_solar_radiation { get; set; }
		public string units_temp { get; set; }
		public string units_wind { get; set; }
	}
}