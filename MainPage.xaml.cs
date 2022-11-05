using Grip.Core.Services.UI;
using Grip.Platforms.Android;
using Debug = System.Diagnostics.Debug;

namespace Grip;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
    }

    private async void addTask_Clicked(object sender, EventArgs e)
    {
        try
        {
            var task = await PromtCRUD.CreateTask();
            if (task == null)
            {
                throw new Exception("Ошибка promt");
            }
            
            await App.DataBase.SaveTaskAsync(task);
            await Shell.Current.DisplayAlert($"Добавить task", $"Сохранено", "ОК");
        }
        catch(Exception ex)
        {
            await Shell.Current.DisplayAlert($"Добавить task", $"Ошибка {ex.Message}", "ОК");
        }
        
    }

    private async void addPeriod_Clicked(object sender, EventArgs e)
    {
        try
        {
            var period = await PromtCRUD.CreatePeriod();
            if (period == null)
            {
                throw new Exception("Ошибка promt");
            }

            await App.DataBase.SavePeriodAsync(period);
            await Shell.Current.DisplayAlert($"Добавить period", $"Сохранено", "ОК");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert($"Добавить period", $"Ошибка {ex.Message}", "ОК");
        }
    }

    private async void runServise_Clicked(object sender, EventArgs e)
    {
        if (!App.IsServiseRunning)
        {
            Android.Content.Intent intent = new Android.Content.Intent(Android.App.Application.Context, typeof(ForegroundServices));
            Android.App.Application.Context.StartForegroundService(intent);
            App.IsServiseRunning = true;
            await Shell.Current.DisplayAlert($"Запуск сервиса", $"Запущено", "ОК");
        }
    }

    private async void stopServise_Clicked(object sender, EventArgs e)
    {
        if (App.IsServiseRunning)
        {
            Android.Content.Intent intent = new Android.Content.Intent(Android.App.Application.Context, typeof(ForegroundServices));
            Android.App.Application.Context.StopService(intent);
            App.IsServiseRunning = false;
            await Shell.Current.DisplayAlert($"Запуск сервиса", $"Остановлено", "ОК");
        }
    }
}

