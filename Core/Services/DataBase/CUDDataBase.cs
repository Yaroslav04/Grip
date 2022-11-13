
namespace Grip.Core.Services.DataBase
{
    public class CUDDataBase<T>
    {
        public SQLiteAsyncConnection connection;
        public async Task<int> SaveAsync(T obj) => await CUD<T>.SaveAsync(obj, connection);
        public async Task<int> UpdateAsync(T obj) => await CUD<T>.UpdateAsync(obj, connection);
        public async Task<int> DeleteAsync(T obj) => await CUD<T>.DeleteAsync(obj, connection);
    }

    public static class CUD<T>
    {
        public static async Task<int> SaveAsync(T obj, SQLiteAsyncConnection _connection)
        {
            try
            {
                return await _connection.InsertAsync(obj);
            }
            catch
            {
                return -1;
            }
        }

        public static async Task<int> DeleteAsync(T obj, SQLiteAsyncConnection _connection)
        {
            try
            {
                return await _connection.DeleteAsync(obj);
            }
            catch
            {
                return -1;
            }
        }

        public static async Task<int> UpdateAsync(T obj, SQLiteAsyncConnection _connection)
        {
            try
            {
                return await _connection.UpdateAsync(obj);
            }
            catch
            {
                return -1;
            }
        }
    }
}
