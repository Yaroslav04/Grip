using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grip.Core.Services.DataBase
{
    public class SensorDataBase : CUDDataBase<SensorClass>
    {
        public SensorDataBase(string _connectionString)
        {
            connection = new SQLiteAsyncConnection(_connectionString);
            connection.CreateTableAsync<SensorClass>().Wait();
        }
    }
}
