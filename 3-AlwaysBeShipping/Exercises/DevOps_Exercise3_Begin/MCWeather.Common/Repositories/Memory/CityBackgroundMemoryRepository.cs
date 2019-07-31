using System;
using System.Linq;
using System.Threading.Tasks;
using MCWeather.Common.Models;
using MCWeather.Common.Repositories.Interfaces;
namespace MCWeather.Common.Repositories.Memory
{
    public class CityBackgroundMemoryRepository : BaseMemoryRepository<CityBackground>, ICityBackgroundRepository
    {
        public async Task<CityBackground> GetItemAsync(string cityName, string weatherOverview)
        {
            await InitializeRepositoryAsync().ConfigureAwait(false);
            return Table.FirstOrDefault(x => x.Name == cityName && x.WeatherOverview == weatherOverview);
        }
    }
}
