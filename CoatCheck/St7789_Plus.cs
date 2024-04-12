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
		const byte SLPIN = 0x10;
		private readonly IDigitalOutputPort _backlightPort;

		public bool IsAwake { get; private set; }

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
			_backlightPort.State = false;
			SendCommand(SLPIN);
			DelayMs(120);
			IsAwake = false;
		}

		public void Wake()
		{
			SendCommand(Register.SLPOUT);
			_backlightPort.State = true;
			IsAwake = true;
		}
	}
}