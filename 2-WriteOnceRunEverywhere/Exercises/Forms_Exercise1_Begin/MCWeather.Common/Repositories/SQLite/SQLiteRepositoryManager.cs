using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCWeather.Common.Repositories.Interfaces;
using SQLite;
using Toolbox.Portable.Infrastructure;
using MCWeather.Common.Models;
using System.IO;
using Toolbox.Portable.Services;
using System.Threading;

namespace MCWeather.Common.Repositories.SQLite
{
    public class SQLiteRepositoryManager : IRepositoryManager
    {
        internal static SQLiteAsyncConnection Database { get; private set; }

        public bool IsInitialized { get; private set; }

        private Lazy<ICityRepository> cityRepository = new Lazy<ICityRepository>(() => new CitySQLiteRepository());
        public ICityRepository CityRepository => cityRepository.Value;

        private Lazy<ICityBackgroundRepository> cityBackgroundRepository = new Lazy<ICityBackgroundRepository>(() => new CityBackgroundSQLiteRepository());
        public ICityBackgroundRepository CityBackgroundRepository => cityBackgroundRepository.Value;

        public async Task DropAllAsync()
        {
            var taskList = new List<Task<int>>
            {
                Database.DropTableAsync<City>(),
                Database.DropTableAsync<CityBackground>()
            };
            await Task.WhenAll(taskList).ConfigureAwait(false);
        }

        private SemaphoreSlim semaphore = new SemaphoreSlim(1);

        public async Task InitializeAsync()
        {
            try
            {
                await semaphore.WaitAsync();

                if (IsInitialized)
                    return;

                IsInitialized = true;

                var databaseName = $"xamLocalStore.db";

                Database = new SQLiteAsyncConnection(Path.Combine(FileSystem.DocumentStorage.BasePath, databaseName));

                await Database.CreateTablesAsync<City, CityBackground>().ConfigureAwait(false);

                await SeedDatabase().ConfigureAwait(false);
            }
            finally
            {
                semaphore.Release();
            }
        }

        private async Task SeedDatabase()
        {
            if ((await Database.Table<City>().CountAsync()) != 0)
                return;

            var citySeedList = new List<City>{
                    new City{
                         Name = "Bellevue"
                    },
                     new City{
                         Name = "San Francisco"
                    },
                     new City{
                         Name = "London"
                    }
                };

            await Database.InsertAllAsync(citySeedList).ConfigureAwait(false);
        }
    }
}
