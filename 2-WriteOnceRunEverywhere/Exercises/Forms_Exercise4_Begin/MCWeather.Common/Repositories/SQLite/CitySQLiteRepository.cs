using System;
using MCWeather.Common.Models;
using MCWeather.Common.Repositories.Interfaces;

namespace MCWeather.Common.Repositories.SQLite
{
    public class CitySQLiteRepository : BaseSQLiteRepository<City>, ICityRepository
    {
    }
}