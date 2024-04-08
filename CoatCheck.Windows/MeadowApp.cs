using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Peripherals.Displays;
using System.Threading.Tasks;

namespace CoatCheck.Windows
{
	public class MeadowApp : App<Desktop>
	{
		readonly IPixelDisplay _display = new WinFormsDisplay(240, 240); // To match the St7789 

		public override Task Initialize()
		{
			Resolver.Log.Info($"Initializing {this.GetType().Name}");
			Resolver.Log.Info($" Platform OS is a {Device.PlatformOS.GetType().Name}");
			Resolver.Log.Info($" Platform: {Device.Information.Platform}");
			Resolver.Log.Info($" OS: {Device.Information.OSVersion}");
			Resolver.Log.Info($" Model: {Device.Information.Model}");
			Resolver.Log.Info($" Processor: {Device.Information.ProcessorType}");

			var displayController = new DisplayController(_display);

			var model = new ViewModel(40, 39);
			displayController.Update(model);

			return base.Initialize();
		}

		public override Task Run()
		{
			// NOTE: this will not return until the display is closed
			ExecutePlatformDisplayRunner();

			return Task.CompletedTask;
		}

		private void ExecutePlatformDisplayRunner()
		{
			if (_display is System.Windows.Forms.Form display)
			{
				// Force the display resolution to match the St7789
				// plus a bit to account for https://github.com/WildernessLabs/Meadow_Issues/issues/564
				display.Width = 256;
				display.Height = 279;

				System.Windows.Forms.Application.Run(display);
			}
		}
	}
}