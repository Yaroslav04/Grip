using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grip.Core.Services.Converter
{
    public static class ClassConverter
    {
        public static ObjectClass ConvertObjectSoketClassToObjectClass(ObjectSoketClass objectSoketClass)
        {
            return new ObjectClass
            {
                N = objectSoketClass.N,
                TaskId = objectSoketClass.TaskId,
                PeriodId = objectSoketClass.PeriodId,
                Descripton = objectSoketClass.Descripton,
                NotificationTime = objectSoketClass.NotificationTime,
                Status = objectSoketClass.Status,
                Day = objectSoketClass.Day,
                SaveDate = objectSoketClass.SaveDate
            };
        }
    }
}
