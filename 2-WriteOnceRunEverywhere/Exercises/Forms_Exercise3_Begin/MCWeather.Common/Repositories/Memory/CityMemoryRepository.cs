using System;
using System.Threading.Tasks;
using MCWeather.Common.Models;
using MCWeather.Common.Repositories.Interfaces;
namespace MCWeather.Common.Repositories.Memory
{
    public class CityMemoryRepository : BaseMemoryRepository<City>, ICityRepository
    {
    }
}
