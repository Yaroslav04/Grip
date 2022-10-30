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
            this.NotificationTime = objectClass.NotificationTime;
            this.Status = objectClass.Status;
            TaskSoket = taskSoket;
            PeriodSoket = periodSoket;
        }
    }
}
