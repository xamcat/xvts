using System.Threading.Tasks;

namespace Toolbox.Portable.Services
{
    public interface ILocationService
    {
        bool HasLocationPermission { get; }
        Task<bool> RequestPermissionAsync();
        Task<LocationInfo> GetCurrentLocationAsync();
        Task<string> GetCityNameFromLocationAsync(LocationInfo location);
    }
}