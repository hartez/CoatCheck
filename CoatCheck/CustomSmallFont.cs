using Meadow.Foundation.Graphics;

namespace CoatCheck
{
	public class CustomSmallFont : IFont
	{
		private readonly IFont _coreFont = new Font8x12();

		// The built-in smaller fonts don't have the ° character in their font tables
		// So we'll use their IFont implementation for most of the characters, and 
		// for the degree symbol we'll emit our own character byte array 
		public byte[] this[char character]
		{
			get
			{
				if(character == 176)
				{
					return _degree;
				}

				return _coreFont[character];
			}
		}

		// Hooray for binary literals! If you squint, you can almost see what the character will look like.
		readonly byte[] _degree = new byte[12]
		{
			0b00000000, 
			0b00001100, 
			0b00010010, 
			0b00010010, 
			0b00001100, 
			0b00000000, 
			0b00000000, 
			0b00000000, 
			0b00000000, 
			0b00000000, 
			0b00000000, 
			0b00000000
		};

		public int Width => _coreFont.Width;

		public int Height => _coreFont.Height;
	}
}
