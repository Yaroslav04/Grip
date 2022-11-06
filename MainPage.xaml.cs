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
        catch (Exception ex)
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

    private async void addObject_Clicked(object sender, EventArgs e)
    {
        string title = "Добавить обьект";

        string _type = await Shell.Current.DisplayActionSheet(title, "Cancel", null, App.TaskTypes.ToArray());
        if (String.IsNullOrWhiteSpace(_type) | _type == "Cancel")
        {
            return;
        }

        var tasks = await App.DataBase.GetTasksAsync(_type);
        if (tasks.Count == 0)
        {
            return;
        }

        Dictionary<int, string> taskDict = new Dictionary<int, string>();
        foreach (var task in tasks)
        {
            taskDict.Add(task.N, task.Name);
        }

        var taskNameList = new List<string>();

        foreach (var s in taskDict.Values)
        {
            taskNameList.Add(s);
        }

        string _task = await Shell.Current.DisplayActionSheet("Выбор задачи", "Cancel", null, taskNameList.ToArray());
        if (String.IsNullOrWhiteSpace(_task) | _task == "Cancel")
        {
            return;
        }

        var tKeyPair = taskDict.Where(x => x.Value == _task).FirstOrDefault();
        int taskId = tKeyPair.Key;

        string _description = await Shell.Current.DisplayPromptAsync(title, $"Описание");

        PeriodClass periodClass = PromtCRUD.GetEmptyPeriod(taskId);

        await App.DataBase.SavePeriodAsync(periodClass);

        var periodId = await App.DataBase.GetPeriodAsync(periodClass);

        try
        {
            await App.DataBase.SaveObjectAsync(
            new ObjectClass
            {
                TaskId = taskId,
                PeriodId = periodId.N,
                Descripton = _description,
                NotificationTime = DateTime.Now.TimeOfDay,
                Status = 1,
                Day = DateTime.Now.DayOfYear,
                SaveDate = DateTime.Now
            });
            await Shell.Current.DisplayAlert($"Создание обьекта", $"Сохранено", "ОК");
        }
        catch (Exception ex)
        {
            try
            {
                await App.DataBase.DeletePeriodAsync(periodId);
            }
            catch
            {
            }

            await Shell.Current.DisplayAlert($"Создание обьекта", $"Ошибка {ex.Message}", "ОК");
        }
    }
}

