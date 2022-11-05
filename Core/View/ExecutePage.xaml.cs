namespace Grip.Core.View;

public partial class ExecutePage : ContentPage
{
	ExecuteViewModel viewModel;

	public ExecutePage()
	{
		InitializeComponent();
		viewModel = new ExecuteViewModel();
		BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.OnAppearing();
    }
}