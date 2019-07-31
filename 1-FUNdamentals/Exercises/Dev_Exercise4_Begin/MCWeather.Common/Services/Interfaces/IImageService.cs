using System;
using System.Threading.Tasks;
using MCWeather.Common.Models;

namespace MCWeather.Common.Services.Interfaces
{
    public interface IImageService
    {
        Task<string> GetBackgroundImageAsync(string city, string weather);
    }
}
