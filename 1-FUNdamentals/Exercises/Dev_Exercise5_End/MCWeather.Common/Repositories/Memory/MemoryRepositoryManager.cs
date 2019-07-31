using System;
using System.Threading.Tasks;
using MCWeather.Common.Models;
using MCWeather.Common.Repositories.Interfaces;
using System.Threading;
namespace MCWeather.Common.Repositories.Memory
{
    public class MemoryRepositoryManager : IRepositoryManager
    {
        public bool IsInitialized { get; private set; }

        private Lazy<ICityRepository> cityRepository = new Lazy<ICityRepository>(() => new CityMemoryRepository());
        public ICityRepository CityRepository => cityRepository.Value;

        private Lazy<ICityBackgroundRepository> cityBackgroundRepository = new Lazy<ICityBackgroundRepository>(() => new CityBackgroundMemoryRepository());
        public ICityBackgroundRepository CityBackgroundRepository => cityBackgroundRepository.Value;

        public Task DropAllAsync()
        {
            return Task.FromResult(true);
        }

        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);

        public async Task InitializeAsync()
        {
            try
            {
                await semaphore.WaitAsync();

                if (IsInitialized) return;
                IsInitialized = true;

                var citySeedList = new[] {
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

                foreach (var city in citySeedList)
                {
                    await CityRepository.InsertAsync(city).ConfigureAwait(false);
                }

            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
