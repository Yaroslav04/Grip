namespace Grip;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(PeriodPage), typeof(PeriodPage));
    }
   
}
