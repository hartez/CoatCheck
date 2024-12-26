using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Foundation.Graphics;
using Meadow;

namespace CoatCheck
{
	class HourControl
	{
		readonly Label _hour;
		readonly Label _conditions0;
		readonly Label _conditions1;
		readonly Label _temp;
		readonly Picture _icon;
		readonly Color _backgroundColor;

		public IControl[] Controls { get; } = new IControl[5];

		public HourControl(Color backgroundColor, int left, int top, int width, int height)
		{
			Controls[0] = _hour = new Label(left, top, width, (height / 4))
			{
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				Font = Text.Small,
				TextColor = Text.DefaultColor
			};

			Controls[1] = _temp = new Label(left + (width / 2), top + (height / 4), (width / 2), (height / 4))
			{
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Left,
				Font = Text.Small,
				TextColor = Text.DefaultColor
			};

			Controls[2] = _icon = new Picture(left, top + (height / 4), (width / 2), (height / 4))
			{
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				BackColor = backgroundColor,
			};

			Controls[3] = _conditions0 = new Label(left, top + (2 * height / 4), width, (height / 4))
			{
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				Font = Text.Tiny,
				TextColor = Text.DefaultColor
			};

			Controls[4] = _conditions1 = new Label(left, top + (3 * height / 4), width, (height / 4))
			{
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				Font = Text.Tiny,
				TextColor = Text.DefaultColor
			};

			_backgroundColor = backgroundColor;
		}

		public void Update(Hourly hourly)
		{
			_hour.Text = hourly.ToHourDisplay();
			_temp.Text = hourly.air_temperature.ToDisplay();

			var conditionLines = hourly.conditions.Split(" ");

			_conditions0.Text = conditionLines[0];

			_conditions1.Text = conditionLines.Length > 1 ? conditionLines[1] : "";

			_icon.Image = ConditionsIconHelpers.GetConditionIconSmall(hourly.icon, _backgroundColor);
		}
	}
}
