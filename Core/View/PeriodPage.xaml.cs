namespace Grip.Core.View;

public partial class PeriodPage : ContentPage
{
	PeriodViewModel viewModel;
	public PeriodPage()
	{
		InitializeComponent();
		viewModel= new PeriodViewModel();
        BindingContext = viewModel;
    }
}