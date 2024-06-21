using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using Meadow.Units;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;
using Meadow.Foundation.Sensors.Motion;

namespace CoatCheck
{
	public class MeadowApp : App<F7FeatherV1>
	{
		CoatCheckSettings _settings;
		string _weatherDataUrl;

		St7789_Plus _display;
		DisplayController _displayController;
		IWiFiNetworkAdapter _wifi;
		bool _isClockSet;

		PushButton _button;

		Timer _weatherUpdateTimer;
		Timer _sleepTimer;

		public override async Task Initialize()
		{
			LogInfo("Initialize started...");

			_settings = new CoatCheckSettings(this);
			_weatherDataUrl = $"https://swd.weatherflow.com/swd/rest/better_forecast?station_id={_settings.StationId}&token={_settings.Token}";

			LogInfo("Past settings...");

			_wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

			_wifi.NetworkError += NetworkError;
			_wifi.NetworkConnected += NetworkConnected;
			_wifi.NetworkDisconnected += NetworkDisconnected;

			var config = new SpiClockConfiguration(
				speed: new Frequency(48000, Frequency.UnitType.Kilohertz),
				mode: SpiClockConfiguration.Mode.Mode3);

			// Set up D10 to run the BLK (backlight) pin on the display 
			var backlight = Device.Pins.D10.CreateDigitalOutputPort(true);

			var spiBus = Device.CreateSpiBus(
				clock: Device.Pins.SCK,
				copi: Device.Pins.MOSI,
				cipo: Device.Pins.MISO,
				config: config);

			_display = new St7789_Plus(
				spiBus: spiBus,
				chipSelectPin: Device.Pins.D02,
				dcPin: Device.Pins.D01,
				resetPin: Device.Pins.D00,
				width: 240, height: 240, backlight);

			_displayController = new DisplayController(_display);

			_sleepTimer = new Timer(TimeSpan.FromMinutes(_settings.SleepTimer).TotalMilliseconds)
			{
				AutoReset = false
			};

			_sleepTimer.Elapsed += (sender, args) => _display.Sleep();
			_sleepTimer.Start();

			var parallaxPir = new ParallaxPir(Device.Pins.D05, InterruptMode.EdgeBoth, ResistorMode.Disabled);

			parallaxPir.OnMotionStart += (sender) => 
			{
				LogInfo($"Motion detected, status is {(_display.IsAwake ? "awake" : "asleep")}");
				_sleepTimer.Stop();
				
				if(!_display.IsAwake)
				{
					_display.Wake();
				}
				
				LogInfo($"Woken up because motion detected, status should now be {(_display.IsAwake ? "awake" : "asleep")}");
			};

			parallaxPir.OnMotionEnd += (sender) => 
			{
				LogInfo($"Motion ended, enabling the sleep timer...");
				_sleepTimer.Start();
			};

			if (_wifi.IsConnected)
			{
				DisplayStatus($"Connected to {_wifi.DefaultSsid}");
				EnsureClockSet();
				await UpdateWeatherData();
			}

			await base.Initialize();
		}

		public override Task Run()
		{
			Resolver.Log.Info("Run...");

			var interval = TimeSpan.FromMinutes(_settings.UpdateInterval).TotalMilliseconds;

			_weatherUpdateTimer = new Timer(interval)
			{
				AutoReset = true
			};

			_weatherUpdateTimer.Elapsed += async (sender, args) =>
			{
				if (_wifi.IsConnected)
				{
					await UpdateWeatherData();
				}
			};

			_weatherUpdateTimer.Start();

			return Task.CompletedTask;
		}

		void EnsureClockSet()
		{
			if(_isClockSet)
			{
				return;
			}

			var now = DateTime.Now;
						
			// The device doesn't know about all the time zones and such, so we'll have to create our own custom time zone
			// To get local times for the display.

			LogInfo($"Device current time is {now}, converting to Mountain...");

			// TODO  We'll fix up the transition rules later
			var zone = TimeZoneInfo.CreateCustomTimeZone("MDT", new TimeSpan(-6, 0, 0), "Mountain Time", "Mountain Standard Time", "Mountain Daylight Time", new TimeZoneInfo.AdjustmentRule[0], true);
			var local = TimeZoneInfo.ConvertTime(now, zone);

			DisplayStatus($"Setting clock to {local:h:mm tt}");

			Device.PlatformOS.SetClock(local);
			_isClockSet = true;
		}

		async Task UpdateWeatherData()
		{
			LogInfo("Checking for updated weather info...");

			using HttpClient client = new HttpClient();

			using HttpResponseMessage response = await client.GetAsync(_weatherDataUrl);

			response.EnsureSuccessStatusCode();
			string responseBody = await response.Content.ReadAsStringAsync();

			LogDebug(responseBody);

			LogInfo("Parsing response...");
			var data = StationData.Parse(responseBody);

			LogInfo("Updating display...");
			_displayController.Update(new WeatherViewModel(data));

			LogInfo("Weather info updated.");
		}

		void NetworkDisconnected(INetworkAdapter sender, NetworkDisconnectionEventArgs args)
		{
			// TODO We need to figure out how the auto reconnect works, and whether to display status updates when this happens
			// we can test it by setting it up on the phone hotspot
		}

		async void NetworkConnected(INetworkAdapter networkAdapter, NetworkConnectionEventArgs args)
		{
			DisplayStatus("Joined network");
			DisplayStatus($"IP Address: {networkAdapter.IpAddress}.");
			DisplayStatus($"Subnet mask: {networkAdapter.SubnetMask}");
			DisplayStatus($"Gateway: {networkAdapter.Gateway}");

			EnsureClockSet();

			DisplayStatus($"Checking the weather...");
			await UpdateWeatherData();
		}

		void NetworkError(INetworkAdapter sender, NetworkErrorEventArgs args)
		{
			DisplayStatus($"Connection error: {args.ErrorCode}");
			LogError($"Network Error: {args.ErrorCode}");
		}

		// TODO it's weird we have to keep track of the state of the display
		// and showing a status at the wrong time means we lose the weather display
		// This might need to be broken into Update(initialization message)
		// and DisplayError(message) or something like that.
		// So we're showing status messages until init is done, and then we only
		// shift away from the weather screen if there's an error

		void DisplayStatus(string message)
		{
			_displayController?.Update(message);
			LogInfo(message);
		}

		static void LogInfo(string message)
		{
			Resolver.Log.Info($"{DateTime.Now.ToLocalTime()}: {message}");
		}

		static void LogDebug(string message)
		{
			Resolver.Log.Debug($"{DateTime.Now.ToLocalTime()}: {message}");
		}

		static void LogError(string message)
		{
			Resolver.Log.Error(message);
		}
	}
}