using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MCWeather.Common.Repositories.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<List<T>> GetItemsAsync();
        Task<T> GetItemAsync(string id);
        Task<T> InsertAsync(T item);
        Task<T> UpdateAsync(T item);
        Task<bool> RemoveAsync(T item);

        Task DropTableAsync();
    }
}