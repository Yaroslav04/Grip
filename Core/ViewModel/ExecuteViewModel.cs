using AndroidX.AppCompat.View.Menu;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            catch
            {

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

            IsBusy = true;
        }
    }
}
