using Meadow;
using System;
using System.Threading.Tasks;

namespace CoatCheck.Windows
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
			ApplicationConfiguration.Initialize();
			await MeadowOS.Start(args);
		}
	}
}