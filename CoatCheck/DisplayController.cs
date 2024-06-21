using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Displays;
using Meadow;
using System;

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
		DisplayMode _mode;

		readonly DisplayScreen _screen;

		readonly Label _temp;
		readonly Label _feelsLike;
		readonly Label _comingUp;
		readonly Label _lastUpdated;
		readonly Picture _currentConditionsIcon;
		readonly Label _currentConditionsLabel;

		readonly HourControl _hour1;
		readonly HourControl _hour2;
		readonly HourControl _hour3;

		int _statusTop = 0;

		Color _statusScreenColor = Color.Black;
		Color _weatherScreenColor = Color.White;

		public DisplayController(IPixelDisplay display)
		{
			_screen = new DisplayScreen(display)
			{
				// TODO Generate background color from temp?
				// We'll need new icons for that without the blending issues in the current ones
				//BackgroundColor = Color.White
			};

			var rowHeight = _screen.Height / 3;
			var screenWidth = _screen.Width;
			int margin = 2;

			Rect currentDest = MarginRect((screenWidth / 2), 0, (screenWidth / 2), rowHeight, marginLeft: 0, marginTop: margin, marginRight: margin, marginBottom: margin);
			Rect currentConditionsIconDest = MarginRect(0, 0, screenWidth / 2, rowHeight, marginLeft: margin, marginTop: margin, marginRight: 0, marginBottom: margin);
			Rect feelsLikeDest = MarginRect(0, rowHeight, screenWidth, rowHeight, margin);

			Rect hourlyDest = MarginRect(0, rowHeight * 2, screenWidth, rowHeight, margin);

			_temp = new Label(currentDest.Left, currentDest.Top + 10, currentDest.Width, (currentDest.Height / 2) - 1)
			{
				Font = Text.Big,
				TextColor = Text.DefaultColor,
				ScaleFactor = ScaleFactor.X2,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Left
			};

			_feelsLike = new Label(feelsLikeDest.Left, feelsLikeDest.Top, width: feelsLikeDest.Width, feelsLikeDest.Height)
			{
				Font = Text.Small,
				TextColor = Text.DefaultColor,
				ScaleFactor = ScaleFactor.X1,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Center
			};

			_comingUp = new Label(feelsLikeDest.Left, feelsLikeDest.Top, width: feelsLikeDest.Width, feelsLikeDest.Height)
			{
				Font = Text.Small,
				TextColor = Text.DefaultColor,
				ScaleFactor = ScaleFactor.X1,
				VerticalAlignment = VerticalAlignment.Bottom,
				HorizontalAlignment = HorizontalAlignment.Center,
				Text = "Coming up:"
			};

			_lastUpdated = new Label(feelsLikeDest.Left, feelsLikeDest.Top, width: feelsLikeDest.Width, feelsLikeDest.Height)
			{
				Font = Text.Small,
				TextColor = Text.DefaultColor,
				ScaleFactor = ScaleFactor.X1,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				Text = "Coming up:"
			};

			_currentConditionsIcon = new Picture(currentConditionsIconDest.Left, currentConditionsIconDest.Top, currentConditionsIconDest.Width, currentConditionsIconDest.Height)
			{
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				BackColor = Color.Transparent
			};

			_currentConditionsLabel = new Label(currentDest.Left, currentDest.Top + 10 + currentDest.Height / 2, currentDest.Width, (currentDest.Height / 2))
			{
				Font = Text.Small,
				TextColor = Text.DefaultColor,
				ScaleFactor = ScaleFactor.X1,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Left
			};

			_hour1 = new HourControl(_weatherScreenColor, hourlyDest.Left, hourlyDest.Top, hourlyDest.Width / 3, hourlyDest.Height);
			_hour2 = new HourControl(_weatherScreenColor, hourlyDest.Left + hourlyDest.Width / 3, hourlyDest.Top, hourlyDest.Width / 3, hourlyDest.Height);
			_hour3 = new HourControl(_weatherScreenColor, hourlyDest.Left + (2 * hourlyDest.Width / 3), hourlyDest.Top, hourlyDest.Width / 3, hourlyDest.Height);
		}

		public void Update(string status)
		{
			UpdateDisplayMode(DisplayMode.Status);

			var statusLabel = CreateStatusLabel();
			_screen.Controls.Add(statusLabel);

			statusLabel.Text = status;
		}

		public void Update(WeatherViewModel model)
		{
			Resolver.Log.Info($"{DateTime.Now.ToLocalTime()}: Updating display");

			UpdateDisplayMode(DisplayMode.Weather);

			_screen.BeginUpdate();

			_temp.Text = model.Temp;
			_feelsLike.Text = $"Feels like {model.FeelsLike}";

			_lastUpdated.Text = $"Last updated: {DateTime.Now:hh:mm tt}";
			_currentConditionsLabel.Text = model.CurrentConditions;

			UpdateCurrentConditionIcon(model.CurrentIcon);

			_hour1.Update(model.Hour1);
			_hour2.Update(model.Hour2);
			_hour3.Update(model.Hour3);

			_screen.EndUpdate();
		}

		Rect MarginRect(int left, int top, int width, int height, int margin)
		{
			return new Rect(left + margin, top + margin, left + width - (2 * margin), top + height - (2 * margin));
		}

		Rect MarginRect(int left, int top, int width, int height, int marginLeft, int marginTop, int marginRight, int marginBottom)
		{
			return new Rect(left + marginLeft, top + marginTop, left + width - (2 * marginRight), top + height - (2 * marginBottom));
		}

		Label CreateStatusLabel()
		{
			var statusLabel = new Label(left: 5, top: _statusTop, width: _screen.Width - 5, height: Text.Status.Height + 2)
			{
				Font = Text.Status,
				TextColor = Color.White,
				ScaleFactor = ScaleFactor.X1,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Left
			};

			_statusTop += statusLabel.Height;

			return statusLabel;
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
				_screen.BackgroundColor = _statusScreenColor;
			}
			else if (displayMode == DisplayMode.Weather)
			{
				_screen.BackgroundColor = _weatherScreenColor;
				_screen.Controls.Add(_currentConditionsIcon);
				_screen.Controls.Add(_temp);
				_screen.Controls.Add(_feelsLike);
				_screen.Controls.Add(_lastUpdated);
				_screen.Controls.Add(_comingUp);
				_screen.Controls.Add(_currentConditionsLabel);

				_screen.Controls.Add(_hour1.Controls);
				_screen.Controls.Add(_hour2.Controls);
				_screen.Controls.Add(_hour3.Controls);
			}

			_mode = displayMode;
		}

		void UpdateCurrentConditionIcon(string icon)
		{
			_currentConditionsIcon.Image = ConditionsIconHelpers.GetConditionImage(icon, 64, _screen.BackgroundColor);
		}
	}
}
