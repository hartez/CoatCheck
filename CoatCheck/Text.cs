using Meadow;
using Meadow.Foundation.Graphics;

namespace CoatCheck
{
	public static class Text
	{
		public static IFont Big { get; } = new Font12x20();
		public static IFont Small { get; } = new CustomSmallFont();
		public static IFont Tiny { get; } = new Font8x8();

		public static IFont Status { get; } = new Font8x12();

		public static Color DefaultColor { get; } = Color.Black;
	}
}
