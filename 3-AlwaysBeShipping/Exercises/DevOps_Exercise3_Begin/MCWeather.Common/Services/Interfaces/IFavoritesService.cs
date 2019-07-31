using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MCWeather.Common.Models;

namespace MCWeather.Common.Services.Interfaces
{
    public interface IFavoritesService
    {
        Task<List<City>> GetFavoriteCitiesAsync();
        Task<City> AddFavoriteCityAsync(City city);
    }
}