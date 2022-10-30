using Debug = System.Diagnostics.Debug;

namespace Grip;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		
    }

    private async Task Add()
    {

        //1
        await App.DataBase.SaveTaskAsync(new TaskClass
        {
            Type = 1,
            Name = "Левофлоксацин",
            IsActive = true,
            SaveDate = DateTime.Now
        });

        await App.DataBase.SavePeriodAsync(new PeriodClass
        {
            Id = 1,
            ControlTime = TimeSpan.FromMinutes(9),
            StopTime = TimeSpan.FromHours(14),
            IsActive = true
        });

        await App.DataBase.SavePeriodAsync(new PeriodClass
        {
            Id = 1,
            ControlTime = TimeSpan.FromHours(14),
            StopTime = TimeSpan.FromHours(19),
            IsActive = true
        });

        await App.DataBase.SavePeriodAsync(new PeriodClass
        {
            Id = 1,
            ControlTime = TimeSpan.FromHours(19),
            StopTime = TimeSpan.FromHours(22),
            IsActive = true
        });

    //2
        await App.DataBase.SaveTaskAsync(new TaskClass
        {
            Type = 1,
            Name = "Назонекс",
            IsActive = true,
            SaveDate = DateTime.Now
        });

        await App.DataBase.SavePeriodAsync(new PeriodClass
        {
            Id = 2,
            ControlTime = TimeSpan.FromMinutes(9) + TimeSpan.FromMinutes(30),
            StopTime = TimeSpan.FromHours(14) + TimeSpan.FromMinutes(30),
            IsActive = true
        });

        await App.DataBase.SavePeriodAsync(new PeriodClass
        {
            Id = 2,
            ControlTime = TimeSpan.FromHours(14) + TimeSpan.FromMinutes(30),
            StopTime = TimeSpan.FromHours(19) + TimeSpan.FromMinutes(30),
            IsActive = true
        });

        await App.DataBase.SavePeriodAsync(new PeriodClass
        {
            Id = 2,
            ControlTime = TimeSpan.FromHours(19) + TimeSpan.FromMinutes(30),
            StopTime = TimeSpan.FromHours(22) + TimeSpan.FromMinutes(30),
            IsActive = true
        });

        //3
        await App.DataBase.SaveTaskAsync(new TaskClass
        {
            Type = 1,
            Name = "Дексаметазон",
            IsActive = true,
            SaveDate = DateTime.Now
        });

        await App.DataBase.SavePeriodAsync(new PeriodClass
        {
            Id = 3,
            ControlTime = TimeSpan.FromMinutes(10),
            StopTime = TimeSpan.FromHours(15),
            IsActive = true
        });

        await App.DataBase.SavePeriodAsync(new PeriodClass
        {
            Id = 3,
            ControlTime = TimeSpan.FromHours(15),
            StopTime = TimeSpan.FromHours(20),
            IsActive = true
        });

        await App.DataBase.SavePeriodAsync(new PeriodClass
        {
            Id = 3,
            ControlTime = TimeSpan.FromHours(20),
            StopTime = TimeSpan.FromHours(23),
            IsActive = true
        });

        //4

        await App.DataBase.SaveTaskAsync(new TaskClass
        {
            Type = 1,
            Name = "Медетром",
            IsActive = true,
            SaveDate = DateTime.Now
        });

        await App.DataBase.SavePeriodAsync(new PeriodClass
        {
            Id = 4,
            ControlTime = TimeSpan.FromMinutes(10) + TimeSpan.FromMinutes(30),
            StopTime = TimeSpan.FromHours(15) + TimeSpan.FromMinutes(30),
            IsActive = true
        });

        await App.DataBase.SavePeriodAsync(new PeriodClass
        {
            Id = 4,
            ControlTime = TimeSpan.FromHours(15) + TimeSpan.FromMinutes(30),
            StopTime = TimeSpan.FromHours(20) + TimeSpan.FromMinutes(30),
            IsActive = true
        });

        await App.DataBase.SavePeriodAsync(new PeriodClass
        {
            Id = 4,
            ControlTime = TimeSpan.FromHours(20) + TimeSpan.FromMinutes(30),
            StopTime = TimeSpan.FromHours(23),
            IsActive = true
        });

    }

    async void Run()
	{
        //await Add();

        foreach (var obj in await App.DataBase.GetObjectsAsync())
        {
            if (obj.Status == 0)
            {
                var task = await App.DataBase.GetTaskAsync(obj.TaskId);
                bool answer = await DisplayAlert($"{task.Name}", $"Виконано {task.Name}", "Так", "Ні");
                if (answer)
                {
                    obj.Status = 1;
                    await App.DataBase.UpdateObjectAsync(obj);
                }
            }
        }
    }

    private void run_Clicked(object sender, EventArgs e)
    {
        Run();
    }
}

