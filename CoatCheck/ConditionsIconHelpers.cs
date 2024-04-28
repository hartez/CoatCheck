using Meadow.Foundation.Graphics;
using Meadow;

namespace CoatCheck
{
	public static class ConditionsIconHelpers
	{
		static void Recolor(Image image, Color source, Color destination)
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

		public static Image GetConditionImage(string icon, int size, Color backgroundColor)
		{
			var name = GetImageResourceName(icon, size);
			var image = Image.LoadFromResource(name);

			// BMP is all that's supported, and the display doesn't support transparency.
			// So we've made the icon transparent backgrounds all solid white. We'll fake the transparency
			// by replacing the white with the specified background color (which we generally get from the DisplayScreen)
			//Recolor(image, Color.Black, backgroundColor);

			return image;
		}

		static string GetImageResourceName(string icon, int size)
		{
			icon = MapConditionIcon(icon);
			return $"CoatCheck.{icon}_{size}.bmp";
		}

		static string MapConditionIcon(string icon)
		{
			// We don't have icons for the "possibly" statuses yet, so strip those values out for now
			icon = icon.Replace("possibly-", "");

			switch (icon)
			{
				// These we have icons for
				case "clear-day":
				case "clear-night":
				case "cloudy":
				case "rainy":
				case "snow":
				case "thunderstorm":
				case "partly-cloudy-day":
					return icon;

				// The rest of these, we don't currently have specific icons for
				case "sleet":
				case "rainy-day":
				case "rainy-night":
				case "sleet-day":
				case "sleet-night":
					return "rainy";

				case "partly-cloudy-night":
				case "windy":
				case "foggy":
					return "cloudy";

				case "snow-day":
				case "snow-night":
					return "snow";

				case "thunderstorm-day":
				case "thunderstorm-night":
					return "thunderstorm";

				default:
					// In case the API returns something unexpected, we don't crash trying to load an icon
					// TODO Create some sort of question mark icon for this case
					return "cloudy";
			};
		}
	}
}
