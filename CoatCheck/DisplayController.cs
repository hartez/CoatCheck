using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Displays;
using Meadow;
using System;

namespace CoatCheck
{
	public class DisplayController
	{
		private readonly DisplayScreen _screen;
		private readonly Label _temp;

		public DisplayController(IPixelDisplay display, ViewModel model = null)
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

			_screen.Controls.Add(_temp);

			if(model != null)
			{
				Update(model); 
			}
		}

		public void Update(ViewModel model)
		{
			_temp.Text = $"{model.Temp} ({model.FeelsLike})";
		}
	}

	public class ViewModel
	{
		public ViewModel(StationData stationData)
		{
			var ob = stationData.obs[0];

			Temp = ob.air_temperature.ToDisplay();
			FeelsLike = ob.feels_like.ToDisplay();
		}

		public ViewModel(double airTemp, double feelsLike)
		{	
			Temp = airTemp.ToDisplay();
			FeelsLike = feelsLike.ToDisplay();
		}

		public string Temp { get; }
		public string FeelsLike { get; }
	}
}
