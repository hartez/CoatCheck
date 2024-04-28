﻿using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using Meadow.Units;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoatCheck
{
	public class MeadowApp : App<F7FeatherV1>
	{
		St7789_Plus _display;
		DisplayController _displayController;

		string _weatherDataUrl;

		bool _wifiConnected;

		CoatCheckSettings _settings;
 
		PushButton _button;

		System.Timers.Timer _weatherUpdateTimer;
		System.Timers.Timer _sleepTimer;

		public override async Task Initialize()
		{
			Info("Initialize...");

			_settings = new CoatCheckSettings();

			var config = new SpiClockConfiguration(
				speed: new Frequency(48000, Frequency.UnitType.Kilohertz),
				mode: SpiClockConfiguration.Mode.Mode3);

			// Set up D10 to run the BLK pin on the display 
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

			_weatherDataUrl = $"https://swd.weatherflow.com/swd/rest/better_forecast?station_id={_settings.StationId}&token={_settings.Token}";

			await SetupNetwork();

			_sleepTimer = new System.Timers.Timer(TimeSpan.FromMinutes(_settings.SleepTimer).TotalMilliseconds)
			{
				AutoReset = false
			};

			_sleepTimer.Elapsed += (sender, args) => _display.Sleep();
			_sleepTimer.Start();

			// Using this button as a temporary stand-in for the motion sensor
			_button = new PushButton(Device.Pins.D05);

			_button.Clicked += (s, a) => 
			{ 
				Resolver.Log.Info($"Sleep/wake button pushed, status is {(_display.IsAwake ? "awake" : "asleep")}");

				if (_display.IsAwake)
				{
					_display.Sleep();
				}
				else
				{
					_display.Wake();
					_sleepTimer.Start();
				}

				Resolver.Log.Info($"Sleep/wake button pushed, status should now be {(_display.IsAwake ? "awake" : "asleep")}");
			};

			await base.Initialize();
		}

		public override Task Run()
		{
			Resolver.Log.Info("Run...");

			var interval = TimeSpan.FromMinutes(_settings.UpdateInterval).TotalMilliseconds;

			_weatherUpdateTimer = new System.Timers.Timer(interval)
			{
				AutoReset = true
			};

			_weatherUpdateTimer.Elapsed += async (sender, args) => 
			{ 
				if(_wifiConnected)
				{
					await UpdateWeatherData();
				}
			};
			
			_weatherUpdateTimer.Start();
			return Task.CompletedTask;
		}

		Task UpdateClock()
		{
			var now = DateTime.Now;
						
			// The device doesn't know about all the time zones and such, so we'll have to create our own custom time zone
			// To get local times for the display.

			// TODO  We'll fix up the transition rules later
			var zone = TimeZoneInfo.CreateCustomTimeZone("MDT", new TimeSpan(-6, 0, 0), "Mountain Time", "Mountain Standard Time", "Mountain Daylight Time", new TimeZoneInfo.AdjustmentRule[0], true);
			var local = TimeZoneInfo.ConvertTime(now, zone);

			_displayController?.Update($"Setting clock to {local}");

			Device.PlatformOS.SetClock(local);

			return Task.CompletedTask;
		}

		async Task UpdateWeatherData()
		{
			Info("Checking for updated weather info...");

			using HttpClient client = new HttpClient();

			HttpResponseMessage response = await client.GetAsync(_weatherDataUrl);
			response.EnsureSuccessStatusCode();
			string responseBody = await response.Content.ReadAsStringAsync();

			Resolver.Log.Info(responseBody);

			var data = StationData.Parse(responseBody);

			_displayController.Update(new WeatherViewModel(data));

			Info("Weather info updated.");
		}

		static void Info(string message)
		{
			Resolver.Log.Info($"{DateTime.Now.ToLocalTime()}: {message}");
		}

		private async Task SetupNetwork()
		{
			var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

			Info($"Registering for network events");

			wifi.NetworkError += NetworkError;
			wifi.NetworkConnected += NetworkConnected;
			wifi.NetworkDisconnected += NetworkDisconnected;
			
			if(wifi.IsConnected)
			{
				_displayController.Update($"Connected to {wifi.DefaultSsid}");
				_wifiConnected = true;
				await UpdateClock();
				await UpdateWeatherData();
			}
			else
			{
				_displayController.Update($"Connecting to {wifi.DefaultSsid}");
				
				try
				{ 
					wifi.ConnectToDefaultAccessPoint();
				}
				catch(Exception ex)
				{
					Resolver.Log.Error($"Network Error: {ex}");
				}
			}
		}

		private void NetworkDisconnected(INetworkAdapter sender, NetworkDisconnectionEventArgs args)
		{
			_wifiConnected = true;
			// TODO We need to figure out how the auto reconnect works, and whether to display status updates when this happens
			// we can test it by setting it up on the phone hotspot
		}

		private async void NetworkConnected(INetworkAdapter networkAdapter, NetworkConnectionEventArgs args)
		{
			Info("Joined network");
			Info($"IP Address: {networkAdapter.IpAddress}.");
			Info($"Subnet mask: {networkAdapter.SubnetMask}");
			Info($"Gateway: {networkAdapter.Gateway}");

			_displayController.Update($"Connected!");
			_displayController.Update($"Checking the weather...");

			await UpdateClock();
			await UpdateWeatherData();

			_wifiConnected = true;
		}

		private void NetworkError(INetworkAdapter sender, NetworkErrorEventArgs args)
		{
			_displayController.Update($"Connection error: {args.ErrorCode}");
			Resolver.Log.Error($"Network Error: {args.ErrorCode}");
		}
	}
}