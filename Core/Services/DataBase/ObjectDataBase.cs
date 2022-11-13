using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grip.Core.Services.DataBase
{
    public class ObjectDataBase : CUDDataBase<ObjectClass>
    {
        public ObjectDataBase(string _connectionString)
        {
            connection = new SQLiteAsyncConnection(_connectionString);
            connection.CreateTableAsync<ObjectClass>().Wait();
        }

        public async Task<List<ObjectClass>> GetObjectsAsync()
        {
            return await connection.Table<ObjectClass>().ToListAsync();
        }

        public async Task<ObjectClass> GetObjectAsync(int _id)
        {
            return await connection.Table<ObjectClass>().Where(x => x.N == _id).FirstOrDefaultAsync();
        }

        public async Task<ObjectClass> GetObjectAsync(int _taskId, int _periodId, int _day)
        {
            return await connection.Table<ObjectClass>()
                .Where(x => x.TaskId == _taskId & x.PeriodId == _periodId
                & x.Day == _day).FirstOrDefaultAsync();
        }

        public async Task<bool> IsObjectExistAsync(int _taskId, int _periodId, int _day)
        {
            var s = await connection.Table<ObjectClass>()
            .Where(x => x.TaskId == _taskId & x.PeriodId == _periodId
            & x.Day == _day).ToListAsync();

            if (s.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
