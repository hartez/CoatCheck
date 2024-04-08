using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays;
using Meadow.Hardware;
using Meadow.Units;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoatCheck
{
	public class MeadowApp : App<F7FeatherV1>
	{
		DisplayController _displayController;
		string _weatherDataUrl;

		public override Task Initialize()
		{
			Info("Initialize...");

			var settings = new CoatCheckSettings();

			var config = new SpiClockConfiguration(
				speed: new Frequency(48000, Frequency.UnitType.Kilohertz),
				mode: SpiClockConfiguration.Mode.Mode3);

			var spiBus = Device.CreateSpiBus(
				clock: Device.Pins.SCK,
				copi: Device.Pins.MOSI,
				cipo: Device.Pins.MISO,
				config: config);

			var display = new St7789(
				spiBus: spiBus,
				chipSelectPin: Device.Pins.D02,
				dcPin: Device.Pins.D01,
				resetPin: Device.Pins.D00,
				width: 240, height: 240);

			_displayController = new DisplayController(display);

			_weatherDataUrl = $"https://swd.weatherflow.com/swd/rest/observations/station/{settings.StationId}?token={settings.Token}";

			var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

			Info($"Registering for network stuff, trying to connect to {wifi.Ssid} default is {wifi.DefaultSsid}");

			wifi.NetworkError += (sender, args) =>
			{
				Resolver.Log.Error($"Network Error: {args.ErrorCode}");
			};

			wifi.NetworkConnected += async (networkAdapter, networkConnectionEventArgs) =>
			{
				Info("Joined network");
				Info($"IP Address: {networkAdapter.IpAddress}.");
				Info($"Subnet mask: {networkAdapter.SubnetMask}");
				Info($"Gateway: {networkAdapter.Gateway}");

				await UpdateWeatherData();
			};

			wifi.ConnectToDefaultAccessPoint();

			return base.Initialize();
		}

		public override Task Run()
		{
			Resolver.Log.Info("Run...");

			return Task.CompletedTask;
		}

		async Task UpdateWeatherData()
		{
			using HttpClient client = new HttpClient();

			HttpResponseMessage response = await client.GetAsync(_weatherDataUrl);
			response.EnsureSuccessStatusCode();
			string responseBody = await response.Content.ReadAsStringAsync();

			Resolver.Log.Info(responseBody);

			var data = StationData.Parse(responseBody);

			_displayController.Update(new ViewModel(data));
		}

		static void Info(string message)
		{
			Resolver.Log.Info(message);
		}
	}
}