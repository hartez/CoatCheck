using Meadow;

namespace CoatCheck
{
	public class CoatCheckSettings : ConfigurableObject
{
    public string Token => GetConfiguredString(nameof(Token));
    public string StationId => GetConfiguredString(nameof(StationId));
}
}