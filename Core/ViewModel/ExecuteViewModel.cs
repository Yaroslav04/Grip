using AndroidX.AppCompat.View.Menu;


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Icu.Text.CaseMap;

namespace Grip.Core.ViewModel
{
    public class ExecuteViewModel : BaseViewModel
    {
        public ExecuteViewModel()
        {
            Items = new ObservableCollection<ObjectSoketClass>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemTapped = new Command<ObjectSoketClass>(Tapped);
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        public ObservableCollection<ObjectSoketClass> Items { get; }

        public Command LoadItemsCommand { get; }
        public Command<ObjectSoketClass> ItemTapped { get; }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            Items.Clear();
            try
            {
                List<ObjectSoketClass> list = new List<ObjectSoketClass>();
                foreach(var obj in await App.DataBase.GetObjectsAsync())
                {
                    TaskClass taskClass = await App.DataBase.GetTaskAsync(obj.TaskId);
                    PeriodClass periodClass = await App.DataBase.GetPeriodAsync(obj.PeriodId);
                    ObjectSoketClass objectSoketClass = new ObjectSoketClass(obj, taskClass,periodClass);
                    list.Add(objectSoketClass);
                }
                var grouped = list.GroupBy(x => x.Day);
                grouped = grouped.Reverse();
                foreach(var g in grouped)
                {
                    List<ObjectSoketClass> sublist = new List<ObjectSoketClass>();
                    foreach (var item in g)
                    {
                        sublist.Add(item);
                    } 
                    
                    sublist = sublist.OrderByDescending(x => x.NotificationTime).ToList();
                    foreach(var i in sublist)
                    {
                        Items.Add(i);
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
        async void Tapped(ObjectSoketClass item)
        {
            if (item == null)
                return;

            string _choise = await Shell.Current.DisplayActionSheet("Выберите действие", "Cancel", null, 
                new string[] {"Смена статуса обьекта", "Клонировать обьект", "Удалить обьект",
                    "Редактировать задачу", "Удалить задачу"});
            if (String.IsNullOrWhiteSpace(_choise) | _choise == "Cancel")
            {
                return;
            }

            if (_choise == "Смена статуса обьекта")
            {
                if (item.Status == 0)
                {
                    var task = await App.DataBase.GetTaskAsync(item.TaskId);
                    bool answer = await Shell.Current.DisplayAlert($"{task.Name}", $"Виконано {task.Name}?", "Так", "Ні");
                    if (answer)
                    {
                        ObjectClass obj = await App.DataBase.GetObjectAsync(item.N);
                        obj.Status = 1;
                        await App.DataBase.UpdateObjectAsync(obj);
                    }
                }

                if (item.Status == 1)
                {
                    var task = await App.DataBase.GetTaskAsync(item.TaskId);
                    bool answer = await Shell.Current.DisplayAlert($"{task.Name}", $"Не виконано {task.Name}?", "Так", "Ні");
                    if (answer)
                    {
                        ObjectClass obj = await App.DataBase.GetObjectAsync(item.N);
                        obj.Status = 2;
                        await App.DataBase.UpdateObjectAsync(obj);
                    }
                }

                if (item.Status == 2)
                {
                    var task = await App.DataBase.GetTaskAsync(item.TaskId);
                    bool answer = await Shell.Current.DisplayAlert($"{task.Name}", $"Виконано {task.Name}?", "Так", "Ні");
                    if (answer)
                    {
                        ObjectClass obj = await App.DataBase.GetObjectAsync(item.N);
                        obj.Status = 1;
                        await App.DataBase.UpdateObjectAsync(obj);
                    }
                }
            }

            if (_choise == "Клонировать обьект")
            {
                bool answer = await Shell.Current.DisplayAlert($"Клонирование", $"Клонировать {item.TaskSoket.Type} {item.TaskSoket.Name}?", "Так", "Ні");
                if (answer)
                {
                    string _description = await Shell.Current.DisplayPromptAsync("Клонирование", $"Описание", maxLength: 100);
                    ObjectClass obj = ClassConverter.ConvertObjectSoketClassToObjectClass(item);
                    obj.Descripton = _description;
                    obj.Status = 1;
                    obj.NotificationTime = DateTime.Now.TimeOfDay;
                    obj.SaveDate = DateTime.Now;
                    obj.Day = DateTime.Now.DayOfYear;

                    PeriodClass periodClass = PromtCRUD.GetEmptyPeriod(obj.TaskId);

                    await App.DataBase.SavePeriodAsync(periodClass);

                    var periodId = await App.DataBase.GetPeriodAsync(periodClass);
                    obj.PeriodId = periodId.N;

                    try
                    {
                        await App.DataBase.SaveObjectAsync(obj);
                        await Shell.Current.DisplayAlert($"Клонирование", $"Сохранено", "ОК");
                    }
                    catch(Exception ex)
                    {
                        await Shell.Current.DisplayAlert($"Клонирование", $"Ошибка {ex.Message}", "ОК");
                    }            
                }
            }

            if (_choise == "Удалить обьект")
            {
                bool answer = await Shell.Current.DisplayAlert($"Удалить", $"Удалить {item.TaskSoket.Type} {item.TaskSoket.Name} {item.SaveDate}?", "Так", "Ні");
                if (answer)
                {
                    try
                    {
                        await App.DataBase.DeleteObjectAsync(ClassConverter.ConvertObjectSoketClassToObjectClass(item));
                        await Shell.Current.DisplayAlert($"Удаление", $"Удалено", "ОК");
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlert($"Удаление", $"Ошибка {ex.Message}", "ОК");
                    }
                }
            }

            if (_choise == "Редактировать задачу")
            {
                bool answer = await Shell.Current.DisplayAlert($"Редактировать", $"Редактировать {item.TaskSoket.Type} {item.TaskSoket.Name}?", "Так", "Ні");
                if (answer)
                {
                    try
                    {
                        await App.DataBase.UpdateTaskAsync(await PromtCRUD.UpdateTask(item.TaskSoket));
                        await Shell.Current.DisplayAlert($"Редактирование", $"Сохранено", "ОК");
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlert($"Редактирование", $"Ошибка {ex.Message}", "ОК");
                    }
                }
            }

            if (_choise == "Удалить задачу")
            {
                bool answer = await Shell.Current.DisplayAlert($"Удалить", $"Удалить {item.TaskSoket.Type} {item.TaskSoket.Name}?", "Так", "Ні");
                if (answer)
                {
                    try
                    {
                        await App.DataBase.DeleteTaskAsync(item.TaskSoket);
                        await Shell.Current.DisplayAlert($"Удаление", $"Удалено", "ОК");
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlert($"Удаление", $"Ошибка {ex.Message}", "ОК");
                    }
                }
            }

            IsBusy = true;
        }
    }
}
