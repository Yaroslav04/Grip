using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grip.Core.Model
{
    public  class SensorClass
    {
        [AutoIncrement]
        [PrimaryKey]
        [NotNull]
        public int N { get; set; }
        public string Sensor { get; set; }
        public int Value { get; set; }
        public DateTime SaveDate { get; set; }
        public string DateToShow { get; set; }
    }
}
