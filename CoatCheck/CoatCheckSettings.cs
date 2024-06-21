using Meadow;

namespace CoatCheck
{
	public class CoatCheckSettings
	{
		public CoatCheckSettings(IApp app)
		{
			Token = GetString(app, nameof(Token));
			StationId = GetString(app, nameof(StationId));
			UpdateInterval = GetInt(app, nameof(UpdateInterval));
			SleepTimer = GetInt(app, nameof(SleepTimer));
		}

		static string GetString(IApp app, string name)
		{
			return app.Settings[$"CoatCheck.{name}"];
		}

		static int GetInt(IApp app, string name)
		{
			return int.Parse(GetString(app, name));
		}

		public string Token { get; }
		public string StationId { get; }
		public int UpdateInterval { get; }
		public int SleepTimer { get; }
	}
}