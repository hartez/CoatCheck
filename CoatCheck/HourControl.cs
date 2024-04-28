using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Foundation.Graphics;
using Meadow;

namespace CoatCheck
{
	class HourControl
	{
		readonly Label _hour;
		readonly Label _conditions;
		readonly Label _temp;
		readonly Picture _icon;
		readonly Color _backgroundColor;

		public IControl[] Controls { get; } = new IControl[4];

		public HourControl(Color backgroundColor, int left, int top, int width, int height)
		{
			Controls[0] = _hour = new Label(left, top, width, (height / 3))
			{
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				Font = Text.Small,
				TextColor = Text.DefaultColor
			};

			Controls[1] = _temp = new Label(left + (width / 2), top + (height / 3), (width / 2), (height / 3))
			{
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Left,
				Font = Text.Small,
				TextColor = Text.DefaultColor
			};

			Controls[2] = _icon = new Picture(left, top + (height / 3), (width / 2), (height / 3))
			{
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				BackColor = backgroundColor,
			};

			Controls[3] = _conditions = new Label(left, top + (2 * height / 3), width, (height / 3))
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
			_conditions.Text = hourly.conditions.ToShortCondition();

			_icon.Image = ConditionsIconHelpers.GetConditionImage(hourly.icon, 24, _backgroundColor);
		}
	}
}
