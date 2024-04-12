using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Displays;
using Meadow;

namespace CoatCheck
{
	enum DisplayMode
	{
		None,
		Status,
		Weather
	}

	public class DisplayController
	{
		private DisplayMode _mode;

		private readonly DisplayScreen _screen;

		private readonly Label _temp;

		int _statusTop = 0;

		public DisplayController(IPixelDisplay display)
		{
			_screen = new DisplayScreen(display)
			{
				BackgroundColor = Color.FromHex("14607F")
			};

			_temp = new Label(left: 2, top: 2, width: _screen.Width - 2, height: (_screen.Height / 2) - 2)
			{
				Font = new Font12x20(),
				TextColor = Color.White,
				ScaleFactor = ScaleFactor.X1,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Center
			};
		}

		Label CreateStatusLabel()
		{
			// TODO Make this a member so we only need it once
			var font = new Font8x12();

			var statusLabel = new Label(left: 5, top: _statusTop, width: _screen.Width - 5, height: font.Height + 2)
			{
				Font = new Font8x12(),
				TextColor = Color.White,
				ScaleFactor = ScaleFactor.X1,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Left
			};

			_statusTop += statusLabel.Height;

			return statusLabel;
		}

		public void Update(string status)
		{
			if (_mode != DisplayMode.Status)
			{
				_mode = DisplayMode.Status;
				_screen.Controls.Clear();
				_statusTop = 2;
			}

			var statusLabel = CreateStatusLabel();
			_screen.Controls.Add(statusLabel);
			
			statusLabel.Text = status;
		}

		public void Update(WeatherViewModel model)
		{
			if (_mode != DisplayMode.Weather)
			{
				_mode = DisplayMode.Weather;
				_screen.Controls.Clear();
				_screen.Controls.Add(_temp);
			}

			_temp.Text = $"{model.Temp} ({model.FeelsLike})";
		}
	}
}
