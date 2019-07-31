using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MCWeather.Common.Models;
using MCWeather.Common.Repositories.Interfaces;
using Toolbox.Portable.Infrastructure;

namespace MCWeather.Common.Repositories.Memory
{
    public abstract class BaseMemoryRepository<T> : IBaseRepository<T> where T : BaseDataObject, new()
    {
        private IRepositoryManager repositoryManager;

        List<T> table;
        protected List<T> Table => table ?? (table = new List<T>());

        Task _initializeTask;
        Task InitializeTask => _initializeTask ?? (_initializeTask = InitializeRepositoryAsync());

        protected async Task InitializeRepositoryAsync()
        {
            if (repositoryManager == null)
                repositoryManager = ServiceContainer.Resolve<IRepositoryManager>();

            if (!repositoryManager.IsInitialized)
                await repositoryManager.InitializeAsync().ConfigureAwait(false);
        }

        public virtual async Task<T> GetItemAsync(string id)
        {
            await InitializeTask.ConfigureAwait(false);
            return Table.FirstOrDefault(x => x.Id == id);
        }

        public virtual async Task<List<T>> GetItemsAsync()
        {
            await InitializeTask.ConfigureAwait(false);
            return Table;
        }

        public virtual async Task<T> InsertAsync(T item)
        {
            await InitializeTask.ConfigureAwait(false);
            Table.Add(item);
            return item;
        }

        public virtual async Task<bool> RemoveAsync(T item)
        {
            await InitializeTask.ConfigureAwait(false);
            Table.Remove(item);
            return true;
        }

        public virtual async Task<T> UpdateAsync(T item)
        {
            await InitializeTask.ConfigureAwait(false);
            var old = await GetItemAsync(item.Id);
            if (old == null)
                return null;

            Table.Remove(old);
            Table.Add(item);

            return item;
        }

        public async Task DropTableAsync()
        {
            await InitializeTask.ConfigureAwait(false);
            Table.Clear();
        }
    }
}
