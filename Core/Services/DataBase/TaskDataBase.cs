using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grip.Core.Services.DataBase
{
    public class TaskDataBase : CUDDataBase<TaskClass>
    {
        public TaskDataBase(string _connectionString)
        {
            connection = new SQLiteAsyncConnection(_connectionString);
            connection.CreateTableAsync<TaskClass>().Wait();
        }

        public async Task<List<TaskClass>> GetTasksAsync()
        {
            return await connection.Table<TaskClass>().ToListAsync();
        }
        public async Task<TaskClass> GetTaskAsync(int _id)
        {
            return await connection.Table<TaskClass>().Where(x => x.N == _id).FirstOrDefaultAsync();
        }

        public async Task<List<TaskClass>> GetTasksAsync(string _type)
        {
            return await connection.Table<TaskClass>().Where(x => x.Type == _type).ToListAsync();
        }

        public async Task<string> GetTaskNameFromId(int _id)
        {
            var items =  await connection.Table<TaskClass>().Where(x => x.N == _id).FirstOrDefaultAsync();
            if (items != null)
            {
                return items.Name;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
