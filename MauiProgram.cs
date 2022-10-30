using Grip.Platforms.Android;

namespace Grip;

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

        Android.Content.Intent intent = new Android.Content.Intent(Android.App.Application.Context, typeof(ForegroundServices));
        Android.App.Application.Context.StartForegroundService(intent);

        return builder.Build();
	}
}
