using System.Diagnostics;

namespace GardenApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		//Debug.WriteLine("initializing App Shell");
		InitializeComponent();

		//Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
		Routing.RegisterRoute(nameof(GardenPage), typeof(GardenPage));
		Routing.RegisterRoute(nameof(AreaPage), typeof(AreaPage));
		Routing.RegisterRoute(nameof(GardenObjectPage), typeof(GardenObjectPage));
	}
}
