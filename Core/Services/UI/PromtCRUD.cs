using Android.App;
using Grip.Core.Services.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grip.Core.Services.UI
{
    public static class PromtCRUD
    {
        public static async Task<TaskClass> CreateTask()
        {
            string title = "Добавить елемент";
            string _type = await Shell.Current.DisplayActionSheet(title, "Cancel", null, App.TaskTypes.ToArray());
            if (String.IsNullOrWhiteSpace(_type) | _type == "Cancel")
            {
                return null;
            }

            string _name = await Shell.Current.DisplayPromptAsync(title, $"Название", maxLength: 100);
            if (String.IsNullOrWhiteSpace(_name) | _name == "Cancel")
            {
                return null;
            }

            return new TaskClass
            {
                Type = _type,
                Name = _name,
                IsActive = true,
                SaveDate = DateTime.Now
            };
        }
        public static async Task<TaskClass> UpdateTask(TaskClass taskClass)
        {
            string title = "Редактировать елемент";

            string _name = await Shell.Current.DisplayPromptAsync(title, $"Название", maxLength: 100, initialValue: taskClass.Name);
            if (String.IsNullOrWhiteSpace(_name) | _name == "Cancel")
            {
                return null;
            }
            bool _active = await Shell.Current.DisplayAlert(title, $"Активный?", "Да", "Нет");

                return new TaskClass
                {
                Type = taskClass.Type,
                Name = _name,
                IsActive = _active,
                SaveDate = taskClass.SaveDate
            };
        }
        public static async Task<PeriodClass> CreatePeriod()
        {
            string title = "Добавить период";

            string _type = await Shell.Current.DisplayActionSheet(title, "Cancel", null, App.TaskTypes.ToArray());
            if (String.IsNullOrWhiteSpace(_type) | _type == "Cancel")
            {
                return null;
            }


            var tasks = await App.DataBase.TaskDB.GetTasksAsync(_type);
            if (tasks.Count == 0)
            {
                return null;
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
                return null;
            }

            var tKeyPair = taskDict.Where(x => x.Value == _task).FirstOrDefault();
            int taskId = tKeyPair.Key;

            string _period = await Shell.Current.DisplayActionSheet("Выбор периода", "Cancel", null, App.PeriodTypes.ToArray());
            if (String.IsNullOrWhiteSpace(_period) | _period == "Cancel")
            {
                return null;
            }

            string _startDate = await Shell.Current.DisplayPromptAsync(title, $"StartDate", initialValue: DateTime.Now.ToShortDateString());
            if (String.IsNullOrWhiteSpace(_startDate) | _startDate == "Cancel")
            {
                return null;
            }

            string _endDate = await Shell.Current.DisplayPromptAsync(title, $"EndDate", initialValue: DateTime.MaxValue.ToShortDateString());
            if (String.IsNullOrWhiteSpace(_endDate) | _endDate == "Cancel")
            {
                return null;
            }      

            string _startTime = await Shell.Current.DisplayPromptAsync(title, $"StartTime");
            if (String.IsNullOrWhiteSpace(_startTime) | _startTime == "Cancel")
            {
                return null;
            }

            string _stopTime = await Shell.Current.DisplayPromptAsync(title, $"StopTime");
            if (String.IsNullOrWhiteSpace(_stopTime) | _stopTime == "Cancel")
            {
                return null;
            }

            string _pause = await Shell.Current.DisplayPromptAsync(title, $"Pause", initialValue:"30");
            if (String.IsNullOrWhiteSpace(_pause) | _pause == "Cancel")
            {
                return null;
            }

            bool _active = await Shell.Current.DisplayAlert(title, $"Активный?", "Да", "Нет");

            bool _notify = await Shell.Current.DisplayAlert(title, $"Присылать уведомления?", "Да", "Нет");

            bool _visible = await Shell.Current.DisplayAlert(title, $"Показывать уведомления?", "Да", "Нет");

            bool _autoday = await Shell.Current.DisplayAlert(title, $"Сбрасывать в новом дне?", "Да", "Нет");

            return new PeriodClass
            {
                Id = taskId,
                StartDate = Convert.ToDateTime(_startDate),
                EndDate = Convert.ToDateTime(_endDate),
                Period = PeriodParser.GetIntFromPeriodName(_period),
                StartTime = TimeSpan.Parse(_startTime),
                StopTime = TimeSpan.Parse(_stopTime),
                Pause = Convert.ToInt32(_pause),
                IsActive = _active,
                IsNotify = _notify,
                IsVisible = _visible,
                IsAutoDayEnd = _autoday,
                SaveDate = DateTime.Now
            };
        }
        public static async Task<PeriodClass> UpdatePeriod(PeriodClass periodClass)
        {
            string title = "Редактировать период";
      
            string _period = await Shell.Current.DisplayActionSheet("Выбор периода", "Cancel", null, App.PeriodTypes.ToArray());
            if (String.IsNullOrWhiteSpace(_period) | _period == "Cancel")
            {
                return null;
            }

            string _startDate = await Shell.Current.DisplayPromptAsync(title, $"StartDate", initialValue: periodClass.StartDate.ToShortDateString());
            if (String.IsNullOrWhiteSpace(_startDate) | _startDate == "Cancel")
            {
                return null;
            }

            string _endDate = await Shell.Current.DisplayPromptAsync(title, $"EndDate", initialValue: periodClass.EndDate.ToShortDateString());
            if (String.IsNullOrWhiteSpace(_endDate) | _endDate == "Cancel")
            {
                return null;
            }

            string _startTime = await Shell.Current.DisplayPromptAsync(title, $"StartTime", initialValue: periodClass.StartTime.ToString());
            if (String.IsNullOrWhiteSpace(_startTime) | _startTime == "Cancel")
            {
                return null;
            }

            string _stopTime = await Shell.Current.DisplayPromptAsync(title, $"StopTime", initialValue: periodClass.StopTime.ToString());
            if (String.IsNullOrWhiteSpace(_stopTime) | _stopTime == "Cancel")
            {
                return null;
            }

            string _pause = await Shell.Current.DisplayPromptAsync(title, $"Pause", initialValue: periodClass.Pause.ToString());
            if (String.IsNullOrWhiteSpace(_pause) | _pause == "Cancel")
            {
                return null;
            }

            bool _active = await Shell.Current.DisplayAlert(title, $"Активный?", "Да", "Нет");

            bool _notify = await Shell.Current.DisplayAlert(title, $"Присылать уведомления?", "Да", "Нет");

            bool _visible = await Shell.Current.DisplayAlert(title, $"Показывать уведомления?", "Да", "Нет");

            bool _autoday = await Shell.Current.DisplayAlert(title, $"Сбрасывать в новом дне?", "Да", "Нет");

            return new PeriodClass
            {
                N = periodClass.N,
                Id = periodClass.Id,
                StartDate = Convert.ToDateTime(_startDate),
                EndDate = Convert.ToDateTime(_endDate),
                Period = PeriodParser.GetIntFromPeriodName(_period),
                StartTime = TimeSpan.Parse(_startTime),
                StopTime = TimeSpan.Parse(_stopTime),
                Pause = Convert.ToInt32(_pause),
                IsActive = _active,
                IsNotify = _notify,
                IsVisible = _visible,
                IsAutoDayEnd = _autoday,
                SaveDate = periodClass.SaveDate
            };
        }
        public static PeriodClass GetEmptyPeriod(int _taskId)
        {      
            return new PeriodClass
            {
                Id = _taskId,
                StartDate = Convert.ToDateTime(DateTime.Now),
                EndDate = Convert.ToDateTime(DateTime.Now),
                Period = 0,
                StartTime = DateTime.Now.TimeOfDay,
                StopTime = DateTime.Now.TimeOfDay,
                Pause = 0,
                IsActive = true,
                IsNotify = true,
                IsVisible = true,
                SaveDate = DateTime.Now
            };
        }

    }
}
