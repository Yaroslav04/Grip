using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grip.Core.ViewModel
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class PeriodViewModel : BaseViewModel
    {
        public PeriodViewModel()
        {
            Items = new ObservableCollection<PeriodClass>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemTapped = new Command<PeriodClass>(Tapped);
        }

        public ObservableCollection<PeriodClass> Items { get; }
        public Command<PeriodClass> ItemTapped { get; }
        public Command LoadItemsCommand { get; }

        private int id;
        public int Id
        {
            get => id;
            set
            {
                SetProperty(ref id, value);
                Load();
            }
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                SetProperty(ref name, value);
            }
        }

        private async void Load()
        {
            Name = await App.DataBase.TaskDB.GetTaskNameFromId(Id);
            IsBusy= true;
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            Items.Clear();
            try
            {
                var items = await App.DataBase.PeriodDB.GetPeriodsAsync(Id);
                if (items.Count > 0)
                {
                    items = items.OrderBy(x => x.StartTime).ToList();

                    foreach (var item in items)
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

        async void Tapped(PeriodClass item)
        {

            if (item == null)
                return;

            string _choise = await Shell.Current.DisplayActionSheet("Выберите действие", "Cancel", null,
                new string[] { "Редактировать расписание", "Удалить расписание" });
            if (String.IsNullOrWhiteSpace(_choise) | _choise == "Cancel")
            {
                return;
            }

            if (_choise == "Редактировать расписание")
            {
                var res = await PromtCRUD.UpdatePeriod(item);
                if (res != null) 
                {
                    await App.DataBase.PeriodDB.UpdateAsync(res);
                }
                IsBusy= true;
            }

            if (_choise == "Удалить расписание")
            {
                bool answer = await Shell.Current.DisplayAlert($"Удалить", $"Удалить {item.StartTime.ToString()}", "Так", "Ні");
                if (answer)
                {
                    try
                    {
                        await App.DataBase.PeriodDB.DeleteAsync(item);
                        await Shell.Current.DisplayAlert($"Удаление", $"Удалено", "ОК");
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlert($"Удаление", $"Ошибка {ex.Message}", "ОК");
                    }
                }
                IsBusy = true;
            }          
        }
    }
}
