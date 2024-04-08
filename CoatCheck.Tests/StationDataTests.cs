using CoatCheck;

namespace CoatCheck.Tests
{
	public class StationDataTests
	{
		string SampleData = @"
{
    ""elevation"": 1632.325317382812,
    ""is_public"": true,
    ""latitude"": 39.92026,
    ""longitude"": -105.07665,
    ""obs"": [{
            ""air_density"": 1.03279,
            ""air_temperature"": 6.0,
            ""barometric_pressure"": 827.6,
            ""brightness"": 34226,
            ""delta_t"": 2.5,
            ""dew_point"": 0.7,
            ""feels_like"": 6.0,
            ""heat_index"": 6.0,
            ""lightning_strike_count"": 0,
            ""lightning_strike_count_last_1hr"": 4,
            ""lightning_strike_count_last_3hr"": 5,
            ""lightning_strike_last_distance"": 17,
            ""lightning_strike_last_epoch"": 1712008827,
            ""precip"": 0.0,
            ""precip_accum_last_1hr"": 0.509053,
            ""precip_accum_local_day"": 14.620024,
            ""precip_accum_local_day_final"": 3.383696,
            ""precip_accum_local_yesterday"": 0.0,
            ""precip_accum_local_yesterday_final"": 0.0,
            ""precip_analysis_type_yesterday"": 0,
            ""precip_minutes_local_day"": 58,
            ""precip_minutes_local_yesterday"": 0,
            ""precip_minutes_local_yesterday_final"": 0,
            ""pressure_trend"": ""steady"",
            ""relative_humidity"": 69,
            ""sea_level_pressure"": 1008.4,
            ""solar_radiation"": 286,
            ""station_pressure"": 827.6,
            ""timestamp"": 1712010026,
            ""uv"": 1.51,
            ""wet_bulb_globe_temperature"": 5.7,
            ""wet_bulb_temperature"": 3.5,
            ""wind_avg"": 0.7,
            ""wind_chill"": 6.0,
            ""wind_direction"": 137,
            ""wind_gust"": 2.2,
            ""wind_lull"": 0.0
        }
    ],
    ""outdoor_keys"": [""timestamp"", ""air_temperature"", ""barometric_pressure"", ""station_pressure"", ""pressure_trend"", ""sea_level_pressure"", ""relative_humidity"", ""precip"", ""precip_accum_last_1hr"", ""precip_accum_local_day"", ""precip_accum_local_day_final"", ""precip_accum_local_yesterday_final"", ""precip_minutes_local_day"", ""precip_minutes_local_yesterday_final"", ""wind_avg"", ""wind_direction"", ""wind_gust"", ""wind_lull"", ""solar_radiation"", ""uv"", ""brightness"", ""lightning_strike_last_epoch"", ""lightning_strike_last_distance"", ""lightning_strike_count"", ""lightning_strike_count_last_1hr"", ""lightning_strike_count_last_3hr"", ""feels_like"", ""heat_index"", ""wind_chill"", ""dew_point"", ""wet_bulb_temperature"", ""wet_bulb_globe_temperature"", ""delta_t"", ""air_density""],
    ""public_name"": ""Test Station"",
    ""station_id"": 1234,
    ""station_name"": ""Test Station"",
    ""station_units"": {
        ""units_direction"": ""cardinal"",
        ""units_distance"": ""mi"",
        ""units_other"": ""imperial"",
        ""units_precip"": ""in"",
        ""units_pressure"": ""inhg"",
        ""units_temp"": ""f"",
        ""units_wind"": ""mph""
    },
    ""status"": {
        ""status_code"": 0,
        ""status_message"": ""SUCCESS""
    },
    ""timezone"": ""America/Denver""
}
";

		[Fact]
		public void WebServiceDataIsParsed()
		{
            var name = StationData.Parse(SampleData).station_name;
            Assert.Equal("Test Station", name);
		}

        [Theory]
        [InlineData(0, "32°")]
        [InlineData(100, "212°")]
        [InlineData(1, "34°")]
        [InlineData(10, "50°")]
        [InlineData(29.4, "85°")]
        public void DisplayTemperaturesConvertToRoundNumbers(double celciusValue, string expected)
        {
            var displayTemp = celciusValue.ToDisplay();

            Assert.Equal(expected, displayTemp);
        }
	}
}