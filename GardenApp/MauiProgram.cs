using GardenApp.Drawable;
using GardenApp.LocationService;
using GardenApp.ViewModel;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace GardenApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<LocationTracker>();
		builder.Services.AddSingleton<MapContext>();
		builder.Services.AddTransient<GraphicsDrawable>();
        builder.Services.AddSingleton<GardenVM>();
		builder.Services.AddTransient<AreaVM>();
		builder.Services.AddTransient<GardenObjectVM>();

		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<GardenPage>();
		builder.Services.AddTransient<AreaPage>();
		builder.Services.AddTransient<GardenObjectPage>();


#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
