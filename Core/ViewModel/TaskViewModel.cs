using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grip.Core.ViewModel
{
    public class TaskViewModel : BaseViewModel
    {
        public TaskViewModel()
        {
            Items = new ObservableCollection<TaskClass>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemTapped = new Command<TaskClass>(Tapped);

        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        public ObservableCollection<TaskClass> Items { get; }

        public Command LoadItemsCommand { get; }
        public Command<TaskClass> ItemTapped { get; }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            Items.Clear();
            try
            {
                var items = await App.DataBase.TaskDB.GetTasksAsync();
                if (items.Count > 0)
                {
                    items = items.OrderByDescending(x => x.SaveDate).ToList();
                    foreach(var item in items) 
                    {
                        Items.Add(item);
                    }
                }
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        async void Tapped(TaskClass item)
        {

            if (item == null)
                return;

            string _choise = await Shell.Current.DisplayActionSheet("Выберите действие", "Cancel", null,
                new string[] {"Расписание", "Редактировать задачу", "Удалить задачу"});
            if (String.IsNullOrWhiteSpace(_choise) | _choise == "Cancel")
            {
                return;
            }

            if (_choise == "Расписание")
            {
                await Shell.Current.GoToAsync($"{nameof(PeriodPage)}?{nameof(PeriodViewModel.Id)}={item.N}");
            }

            if (_choise == "Редактировать задачу")
            {
                bool answer = await Shell.Current.DisplayAlert($"Редактировать", $"Редактировать {item.Type} {item.Name}?", "Так", "Ні");
                if (answer)
                {
                    try
                    {
                        await App.DataBase.TaskDB.UpdateAsync(await PromtCRUD.UpdateTask(item));
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
                bool answer = await Shell.Current.DisplayAlert($"Удалить", $"Удалить {item.Type} {item.Name}?", "Так", "Ні");
                if (answer)
                {
                    try
                    {
                        await App.DataBase.TaskDB.DeleteAsync(item);
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
