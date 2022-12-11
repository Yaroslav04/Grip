namespace Grip;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		RunAsync();
		Routing.RegisterRoute(nameof(PeriodPage), typeof(PeriodPage));
    }

	public async void RunAsync()
	{
        App.BTClass.RunAsync();
    }
   
}
