using System;
using MCWeather.Common.Models;
using System.Threading.Tasks;

namespace MCWeather.Common.Repositories.Interfaces
{
    public interface ICityBackgroundRepository : IBaseRepository<CityBackground>
    {
        Task<CityBackground> GetItemAsync(string cityName, string weatherOverview);
    }
}
