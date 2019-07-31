using System;
using System.Threading.Tasks;

namespace MCWeather.Common.Repositories.Interfaces
{
    public interface IRepositoryManager
    {
        bool IsInitialized { get; }
        Task InitializeAsync();
        ICityRepository CityRepository { get; }
        ICityBackgroundRepository CityBackgroundRepository { get; }

        Task DropAllAsync();
    }
}
