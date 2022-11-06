using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grip.Core.Model
{
    public class ObjectSoketClass : ObjectClass
    {
        public TaskClass TaskSoket { get; set; }
        public PeriodClass PeriodSoket { get; set; }
        public ObjectSoketClass(ObjectClass objectClass, TaskClass taskSoket, PeriodClass periodSoket)
        {
            this.N = objectClass.N;
            this.TaskId = objectClass.TaskId;
            this.PeriodId = objectClass.PeriodId;
            this.Descripton = objectClass.Descripton;
            this.NotificationTime = objectClass.NotificationTime;
            this.Status = objectClass.Status;
            this.Day = objectClass.Day;
            this.SaveDate = objectClass.SaveDate;
            TaskSoket = taskSoket;
            PeriodSoket = periodSoket;
        }
    }
}
