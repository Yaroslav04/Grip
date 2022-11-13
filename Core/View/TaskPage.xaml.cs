namespace Grip.Core.View;

public partial class TaskPage : ContentPage
{
	TaskViewModel viewModel;
	public TaskPage()
	{
		InitializeComponent();
		viewModel= new TaskViewModel();
        BindingContext = viewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.OnAppearing();
    }
}