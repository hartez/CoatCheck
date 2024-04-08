using System.Collections.Generic;
using System.Text.Json;

namespace CoatCheck
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "No point in wasting cycles on a JSON naming convention for this request, the data is clear enough")]
	public class Ob
	{
		public double air_density { get; set; }
		public double air_temperature { get; set; }
		public double barometric_pressure { get; set; }
		public int brightness { get; set; }
		public double delta_t { get; set; }
		public double dew_point { get; set; }
		public double feels_like { get; set; }
		public double heat_index { get; set; }
		public int lightning_strike_count { get; set; }
		public int lightning_strike_count_last_1hr { get; set; }
		public int lightning_strike_count_last_3hr { get; set; }
		public int lightning_strike_last_distance { get; set; }
		public int lightning_strike_last_epoch { get; set; }
		public double precip { get; set; }
		public double precip_accum_last_1hr { get; set; }
		public double precip_accum_local_day { get; set; }
		public double precip_accum_local_day_final { get; set; }
		public double precip_accum_local_yesterday { get; set; }
		public double precip_accum_local_yesterday_final { get; set; }
		public int precip_analysis_type_yesterday { get; set; }
		public int precip_minutes_local_day { get; set; }
		public int precip_minutes_local_yesterday { get; set; }
		public int precip_minutes_local_yesterday_final { get; set; }
		public string pressure_trend { get; set; }
		public int relative_humidity { get; set; }
		public double sea_level_pressure { get; set; }
		public int solar_radiation { get; set; }
		public double station_pressure { get; set; }
		public int timestamp { get; set; }
		public double uv { get; set; }
		public double wet_bulb_globe_temperature { get; set; }
		public double wet_bulb_temperature { get; set; }
		public double wind_avg { get; set; }
		public double wind_chill { get; set; }
		public int wind_direction { get; set; }
		public double wind_gust { get; set; }
		public double wind_lull { get; set; }
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "No point in wasting cycles on a JSON naming convention for this request, the data is clear enough")]
	public class StationUnits
	{
		public string units_direction { get; set; }
		public string units_distance { get; set; }
		public string units_other { get; set; }
		public string units_precip { get; set; }
		public string units_pressure { get; set; }
		public string units_temp { get; set; }
		public string units_wind { get; set; }
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "No point in wasting cycles on a JSON naming convention for this request, the data is clear enough")]
	public class Status
	{
		public int status_code { get; set; }
		public string status_message { get; set; }
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "No point in wasting cycles on a JSON naming convention for this request, the data is clear enough")]
	public class StationData
	{
		public double elevation { get; set; }
		public bool is_public { get; set; }
		public double latitude { get; set; }
		public double longitude { get; set; }
		public IList<Ob> obs { get; set; }
		public IList<string> outdoor_keys { get; set; }
		public string public_name { get; set; }
		public int station_id { get; set; }
		public string station_name { get; set; }
		public StationUnits station_units { get; set; }
		public Status status { get; set; }
		public string timezone { get; set; }

		public static StationData Parse(string data)
		{
			return JsonSerializer.Deserialize<StationData>(data);
		}
	}
}
