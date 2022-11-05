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

            string _description = await Shell.Current.DisplayPromptAsync(title, $"Описание", maxLength: 100);

            return new TaskClass
            {
                Type = _type,
                Name = _name,
                Descripton = _description,
                IsActive = true,
                SaveDate = DateTime.Now
            };
        }

        public static async Task<TaskClass> UpdateTask(TaskClass taskClass)
        {
            string title = "Добавить елемент";

            string _name = await Shell.Current.DisplayPromptAsync(title, $"Название", maxLength: 100, initialValue: taskClass.Name);
            if (String.IsNullOrWhiteSpace(_name) | _name == "Cancel")
            {
                return null;
            }

            string _description = await Shell.Current.DisplayPromptAsync(title, $"Описание", maxLength: 100, initialValue: taskClass.Descripton);

            return new TaskClass
            {
                Type = taskClass.Type,
                Name = _name,
                Descripton = _description,
                IsActive = taskClass.IsActive,
                SaveDate = taskClass.SaveDate
            };
        }

        public static async Task<PeriodClass> CreatePeriod()
        {
            string title = "Добавить период";

            //TODO select type

            var tasks = await App.DataBase.GetTasksAsync();
            if (tasks == null)
            {
                return null;
            }

            Dictionary<int, string> taskDict = new Dictionary<int, string>();
            var taskList = new List<string>();
            int i = 1;
            foreach (var t in tasks)
            {
                taskDict.Add(i, t.Name);
                taskList.Add(t.Name);
                i++;
            }

            string _task = await Shell.Current.DisplayActionSheet("Выбор задачи", "Cancel", null, taskList.ToArray());
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

            return new PeriodClass
            {
                Id = taskId,
                StartDate = Convert.ToDateTime(_startDate),
                EndDate = Convert.ToDateTime(_endDate),
                Period = PeriodParser.GetIntFromPeriodName(_period),
                StartTime = TimeSpan.Parse(_startTime),
                StopTime = TimeSpan.Parse(_stopTime),
                Pause = Convert.ToInt32(_pause),
                IsActive = true,
                IsNotify = true,
                IsVisible = true,
                SaveDate = DateTime.Now
            };
        }
    }
}
