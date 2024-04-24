using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Displays;
using Meadow;
using System.Runtime.CompilerServices;

namespace CoatCheck
{
	enum DisplayMode
	{
		None,
		Status,
		Weather
	}

	public enum WeatherCondition
	{
		DayClear,
		NightClear,
		Cloudy,
		PartlyCloudy,
		Rain,
		Snow,
		Thunderstorm
	}

	public class DisplayController
	{
		DisplayMode _mode;

		readonly DisplayScreen _screen;

		readonly Label _temp;
		readonly Label _feelsLike;
		readonly Picture _currentConditionsIcon;

		readonly IFont _bigFont = new Font12x20();
		readonly IFont _smallFont = new CustomSmallFont();

		int _statusTop = 0;

		public DisplayController(IPixelDisplay display)
		{
			_screen = new DisplayScreen(display)
			{
				BackgroundColor = Color.FromHex("14607F")
			};

			var rowHeight = _screen.Height / 3;
			var screenWidth = _screen.Width;
			int margin = 2;

			Rect tempDest = MarginRect((screenWidth / 2), 0, (screenWidth / 2), rowHeight, margin);
			Rect currentConditionsDest = MarginRect(0, 0, screenWidth / 2, rowHeight, 2);
			Rect feelsLikeDest = MarginRect(0, rowHeight, screenWidth, rowHeight, 2);

			_temp = new Label(tempDest.Left, tempDest.Top, tempDest.Width, tempDest.Height)
			{
				Font = _bigFont,
				TextColor = Color.White,
				ScaleFactor = ScaleFactor.X1,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Left
			};

			_feelsLike = new Label(feelsLikeDest.Left, feelsLikeDest.Top, width: feelsLikeDest.Width, feelsLikeDest.Height)
			{
				Font = _smallFont,
				TextColor = Color.White,
				ScaleFactor = ScaleFactor.X1,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center
			};

			_currentConditionsIcon = new Picture(currentConditionsDest.Left, currentConditionsDest.Top, currentConditionsDest.Width, currentConditionsDest.Height)
			{
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Right,
				BackColor = _screen.BackgroundColor
			};
		}

		Rect MarginRect(int left, int top, int width, int height, int margin)
		{
			return new Rect(left + margin, top + margin, left + width - (2 * margin), top + height - (2 * margin));
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
			UpdateDisplayMode(DisplayMode.Weather);

			_temp.Text = model.Temp;
			_feelsLike.Text = $"Feels like {model.FeelsLike}";

			UpdateCurrentConditionIcon(WeatherCondition.Cloudy);
		}

		void UpdateDisplayMode(DisplayMode displayMode)
		{
			if (_mode == displayMode)
			{
				return;
			}

			_screen.Controls.Clear();

			if (displayMode == DisplayMode.Status)
			{
				_statusTop = 2;
			}
			else if (displayMode == DisplayMode.Weather)
			{
				_screen.Controls.Add(_currentConditionsIcon);
				_screen.Controls.Add(_temp);
				_screen.Controls.Add(_feelsLike);
			}

			_mode = displayMode;
		}

		void UpdateCurrentConditionIcon(WeatherCondition condition)
		{
			_currentConditionsIcon.Image = GetConditionImage(condition, 64);
		}

		void RecolorBackground(Image image, Color source, Color destination)
		{
			byte sourceByte = source.Color8bppRgb332;
			var buffer = image.DisplayBuffer;

			int width = image.Width;
			int height = image.Height;

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					var pixel = buffer.GetPixel(x, y);
					if (pixel.Color8bppRgb332 == sourceByte)
					{
						buffer.SetPixel(x, y, destination);
					}
				}
			}
		}

		Image GetConditionImage(WeatherCondition condition, int size)
		{
			var name = GetImageResourceName(condition, size);
			var image = Image.LoadFromResource(name);

			// BMP is all that's supported, and BMP (here at least) doesn't support transparency.
			// So the icon transparent backgrounds all end up solid black. We'll fake the transparency
			// by replacing the black with the _screen background color.
			RecolorBackground(image, Color.Black, _screen.BackgroundColor);

			return image;
		}

		string GetImageResourceName(WeatherCondition condition, int size)
		{
			return $"CoatCheck.{condition.ToString().ToLower()}_{size}.bmp";
		}
	}
}
