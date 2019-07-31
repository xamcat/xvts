using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MCWeather.Common.Repositories.Interfaces;
using MCWeather.Common.Models;
using SQLite;
using Toolbox.Portable.Infrastructure;

namespace MCWeather.Common.Repositories.SQLite
{
    public abstract class BaseSQLiteRepository<T> : IBaseRepository<T> where T : BaseDataObject, new()
    {
        IRepositoryManager _repositoryManager;

        Task _initializeTask;
        Task InitializeTask => _initializeTask ?? (_initializeTask = InitializeRepositoryAsync());

        SQLiteAsyncConnection database;
        protected SQLiteAsyncConnection Database
        {
            get { return database ?? (database = SQLiteRepositoryManager.Database); }
        }

        protected async Task InitializeRepositoryAsync()
        {
            if (_repositoryManager == null)
                _repositoryManager = ServiceContainer.Resolve<IRepositoryManager>();

            if (!_repositoryManager.IsInitialized)
                await _repositoryManager.InitializeAsync().ConfigureAwait(false);
        }

        public virtual async Task<T> GetItemAsync(string id)
        {
            try
            {
                await InitializeTask.ConfigureAwait(false);
                var items = await Database.Table<T>().Where(s => s.Id == id).ToListAsync().ConfigureAwait(false);

                if (items == null || items.Count == 0)
                    return null;

                return items[0];
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public virtual async Task<List<T>> GetItemsAsync()
        {
            try
            {
                await InitializeTask.ConfigureAwait(false);

                return await Database.Table<T>().ToListAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public virtual async Task<T> InsertAsync(T item)
        {
            try
            {
                await InitializeTask.ConfigureAwait(false);
                await Database.InsertAsync(item).ConfigureAwait(false);
                return item;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public virtual async Task<bool> RemoveAsync(T item)
        {
            try
            {
                await InitializeTask.ConfigureAwait(false);
                await Database.DeleteAsync(item).ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public virtual async Task<T> UpdateAsync(T item)
        {
            try
            {
                await InitializeTask.ConfigureAwait(false);
                if ((await Database.UpdateAsync(item).ConfigureAwait(false)) > 0)
                    return item;
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public Task DropTableAsync()
        {
            return Database.DropTableAsync<T>();
        }
    }
}
