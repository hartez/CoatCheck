using Meadow.Foundation.Displays;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;

namespace CoatCheck
{
	/// <summary>
	/// Extends the St7789 class to add Sleep and Wake commands
	/// </summary>
	public class St7789_Plus : St7789
	{
		private readonly IDigitalOutputPort _backlightPort;

		// TODO Once the motion sensor is in place, this might be unnecessary; it's just here so the manual button knows what to toggle
		public bool IsAwake => _backlightPort.State;

		public St7789_Plus(ISpiBus spiBus, IPin chipSelectPin, IPin dcPin, IPin resetPin, int width, int height, 
			IDigitalOutputPort backlightPort, ColorMode colorMode = ColorMode.Format12bppRgb444) 
				: base(spiBus, chipSelectPin, dcPin, resetPin, width, height, colorMode)
		{
			_backlightPort = backlightPort;
		}

		public St7789_Plus(ISpiBus spiBus, IDigitalOutputPort chipSelectPort, IDigitalOutputPort dataCommandPort, 
			IDigitalOutputPort resetPort, int width, int height, ColorMode colorMode = ColorMode.Format12bppRgb444) 
				: base(spiBus, chipSelectPort, dataCommandPort, resetPort, width, height, colorMode)
		{
		}

		public void Sleep()
		{
			// Originally this was also sending the SLPIN (0x10) command, which _should_ be putting the screen in 
			// low power consumption mode. But for some reason, after waking up we can no longer update the screen.
			// So for now, limiting "sleep" mode to just turning off the backlight.

			_backlightPort.State = false;
		}

		public void Wake()
		{
			_backlightPort.State = true;
		}
	}
}