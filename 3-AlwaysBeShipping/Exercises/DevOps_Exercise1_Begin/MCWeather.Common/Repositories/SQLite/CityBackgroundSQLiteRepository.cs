using System;
using MCWeather.Common.Models;
using MCWeather.Common.Repositories.Interfaces;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MCWeather.Common.Repositories.SQLite
{
    public class CityBackgroundSQLiteRepository : BaseSQLiteRepository<CityBackground>, ICityBackgroundRepository
    {
        public async Task<CityBackground> GetItemAsync(string cityName, string weatherOverview)
        {
            try
            {
                await InitializeRepositoryAsync().ConfigureAwait(false);
                var items = await Database.Table<CityBackground>().Where(s => s.Name == cityName && s.WeatherOverview == weatherOverview).ToListAsync().ConfigureAwait(false);

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
    }
}
