using Android.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grip.Core.Services.DataBase
{
    public class DataBase
    {
        public TaskDataBase TaskDB;
        public PeriodDataBase PeriodDB;
        public ObjectDataBase ObjectDB;

        public DataBase(string _connectionString, List<string> _dataBaseName)
        {
            TaskDB = new TaskDataBase(Path.Combine(_connectionString, _dataBaseName[0]));
            PeriodDB = new PeriodDataBase(Path.Combine(_connectionString, _dataBaseName[1]));
            ObjectDB = new ObjectDataBase(Path.Combine(_connectionString, _dataBaseName[2]));
        }
    }
}
