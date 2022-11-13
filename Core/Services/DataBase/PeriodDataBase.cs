using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grip.Core.Services.DataBase
{
    public class PeriodDataBase : CUDDataBase<PeriodClass>
    {
        public PeriodDataBase(string _connectionString)
        {
            connection = new SQLiteAsyncConnection(_connectionString);
            connection.CreateTableAsync<PeriodClass>().Wait();
        }

        public async Task<List<PeriodClass>> GetPeriodsAsync()
        {
            return await connection.Table<PeriodClass>().ToListAsync();
        }

        public async Task<PeriodClass> GetPeriodAsync(int _id)
        {
            return await connection.Table<PeriodClass>().Where(x => x.N == _id).FirstOrDefaultAsync();
        }

        public async Task<PeriodClass> GetPeriodAsync(PeriodClass periodClass)
        {
            return await connection.Table<PeriodClass>().Where(x => x.SaveDate == periodClass.SaveDate & x.Id == periodClass.Id).FirstOrDefaultAsync();
        }

        public async Task<List<PeriodClass>> GetPeriodsAsync(int _id)
        {
            return await connection.Table<PeriodClass>().Where(x => x.Id == _id).ToListAsync();
        }
    }
}
