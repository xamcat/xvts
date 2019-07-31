using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MCWeather.Common.Models;
using MCWeather.Common.Services.Interfaces;
using Toolbox.Portable.Services;
using MCWeather.Common.Repositories.Interfaces;
using Toolbox.Portable.Infrastructure;

namespace MCWeather.Common.Services
{
    public class ImageService : BaseHttpService, IImageService
    {
        IRepositoryManager repositoryManager;
        IRepositoryManager RepositoryManager => repositoryManager ?? (repositoryManager = ServiceContainer.Resolve<IRepositoryManager>());

        public ImageService()
            : base("https://asapinu5ov4uzk6wu6.azurewebsites.net/")
        {
        }

        public async Task<string> GetBackgroundImageAsync(string city, string weather)
        {
            var searchWeather = weather.Replace(" ", "+");

            var cityBackground = await RepositoryManager.CityBackgroundRepository.GetItemAsync(city, searchWeather).ConfigureAwait(false);

            if (cityBackground == null)
            {
                var remoteImage = await GetAsync<CityBackground>($"api/images/{city}", requestMessage =>
                {
                    requestMessage.Headers.Add("key", "148a16d1-a235-4caa-9f0d-f44f90737ad5");
                }).ConfigureAwait(false);
                cityBackground = new CityBackground
                {
                    Name = city,
                    WeatherOverview = searchWeather,
                    ImageUrl = remoteImage.ImageUrl
                };

                await RepositoryManager.CityBackgroundRepository.InsertAsync(cityBackground).ConfigureAwait(false);
            }

            return await WebImageCache.RetrieveImage(cityBackground.ImageUrl, $"{city}-{searchWeather}");
        }
    }
}
