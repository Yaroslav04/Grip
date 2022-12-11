using Grip.Platforms.Android;

namespace Grip;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(PeriodPage), typeof(PeriodPage));
        if (!App.IsServiseRunning)
        {
            Android.Content.Intent intent = new Android.Content.Intent(Android.App.Application.Context, typeof(ForegroundServices));
            Android.App.Application.Context.StartForegroundService(intent);
            App.IsServiseRunning = true;
        }
    }
}
