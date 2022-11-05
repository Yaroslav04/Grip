using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grip.Core.Services.DataBase
{
    public class DataBase
    {
        readonly SQLiteAsyncConnection taskDataBase;
        readonly SQLiteAsyncConnection periodDataBase;
        readonly SQLiteAsyncConnection objectDataBase;
        public DataBase(string _connectionString, List<string> _dataBaseName)
        {

            taskDataBase = new SQLiteAsyncConnection(Path.Combine(_connectionString, _dataBaseName[0]));
            taskDataBase.CreateTableAsync<TaskClass>().Wait();

            periodDataBase = new SQLiteAsyncConnection(Path.Combine(_connectionString, _dataBaseName[1]));
            periodDataBase.CreateTableAsync<PeriodClass>().Wait();

            objectDataBase = new SQLiteAsyncConnection(Path.Combine(_connectionString, _dataBaseName[2]));
            objectDataBase.CreateTableAsync<ObjectClass>().Wait();
        }


        #region Task

        public Task<int> SaveTaskAsync(TaskClass _object)
        {
            try
            {
                return taskDataBase.InsertAsync(_object);
            }
            catch
            {
                return null;
            }
        }
        public Task<int> DeleteTaskAsync(TaskClass _object)
        {
            try
            {
                return taskDataBase.DeleteAsync(_object);
            }
            catch
            {
                return null;
            }

        }
        public Task<int> UpdateTaskAsync(TaskClass _object)
        {
            try
            {
                return taskDataBase.UpdateAsync(_object);
            }
            catch
            {
                return null;
            }

        }

        public async Task<List<TaskClass>> GetTasksAsync()
        {
            return await taskDataBase.Table<TaskClass>().ToListAsync();
        }
        public async Task<TaskClass> GetTaskAsync(int _id)
        {
            return await taskDataBase.Table<TaskClass>().Where(x => x.N == _id).FirstOrDefaultAsync();
        }


        #endregion

        #region Period

        public Task<int> SavePeriodAsync(PeriodClass _object)
        {
            try
            {
                return periodDataBase.InsertAsync(_object);
            }
            catch
            {
                return null;
            }
        }
        public Task<int> DeletePeriodAsync(PeriodClass _object)
        {
            try
            {
                return periodDataBase.DeleteAsync(_object);
            }
            catch
            {
                return null;
            }

        }
        public Task<int> UpdatePeriodAsync(PeriodClass _object)
        {
            try
            {
                return periodDataBase.UpdateAsync(_object);
            }
            catch
            {
                return null;
            }

        }

        public async Task<List<PeriodClass>> GetPeriodsAsync()
        {
            return await periodDataBase.Table<PeriodClass>().ToListAsync();
        }

        public async Task<List<PeriodClass>> GetPeriodsAsync(int _id)
        {
            return await periodDataBase.Table<PeriodClass>().Where(x => x.Id == _id).ToListAsync();
        }

        public async Task<PeriodClass> GetPeriodAsync(int _id)
        {
            return await periodDataBase.Table<PeriodClass>().Where(x => x.N == _id).FirstOrDefaultAsync();
        }

        #endregion

        #region Object

        public Task<int> SaveObjectAsync(ObjectClass _object)
        {
            try
            {
                return objectDataBase.InsertAsync(_object);
            }
            catch
            {
                return null;
            }
        }
        public Task<int> DeleteObjectAsync(ObjectClass _object)
        {
            try
            {
                return objectDataBase.DeleteAsync(_object);
            }
            catch
            {
                return null;
            }

        }
        public Task<int> UpdateObjectAsync(ObjectClass _object)
        {
            try
            {
                return objectDataBase.UpdateAsync(_object);
            }
            catch
            {
                return null;
            }

        }

        public async Task<List<ObjectClass>> GetObjectsAsync()
        {
            return await objectDataBase.Table<ObjectClass>().ToListAsync();
        }

        public async Task<ObjectClass> GetObjectAsync(int _id)
        {
            return await objectDataBase.Table<ObjectClass>().Where(x => x.N == _id).FirstOrDefaultAsync();
        }

        public async Task<ObjectClass> GetObjectAsync(int _taskId, int _periodId, int _day)
        {
            return await objectDataBase.Table<ObjectClass>()
                .Where(x => x.TaskId == _taskId & x.PeriodId == _periodId
                & DateTime.Now.DayOfYear == _day).FirstOrDefaultAsync();
        }

        public async Task<bool> IsObjectExistAsync(int _taskId, int _periodId, int _day)
        {
            var s = await objectDataBase.Table<ObjectClass>()
                .Where(x => x.TaskId  == _taskId & x.PeriodId == _periodId 
                & DateTime.Now.DayOfYear == _day).ToListAsync();

            if (s.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion



    }
}
