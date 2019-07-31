using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCWeather.Common.Models;
using MCWeather.Common.Repositories.Interfaces;
using MCWeather.Common.Services.Interfaces;
using Toolbox.Portable.Services;
using Toolbox.Portable.Infrastructure;

namespace MCWeather.Common.Services
{
    public class FavoritesService : IFavoritesService
    {
        IRepositoryManager repositoryManager;
        IRepositoryManager RepositoryManager => repositoryManager ?? (repositoryManager = ServiceContainer.Resolve<IRepositoryManager>());

        public Task<List<City>> GetFavoriteCitiesAsync()
        {
            return RepositoryManager.CityRepository.GetItemsAsync();
        }

        public async Task<City> AddFavoriteCityAsync(City city)
        {
            if (string.IsNullOrWhiteSpace(city?.Name))
            {
                throw new ArgumentException("City and City Name cannot be null or whitespace.");
            }

            await RepositoryManager.CityRepository.InsertAsync(city).ConfigureAwait(false);

            return city;
        }
    }
}