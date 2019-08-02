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
            : base("https://asapikjy6zobfbf6xe.azurewebsites.net/")
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
                    requestMessage.Headers.Add("key", "B50996A0-9D60-44AF-BF08-81029CE2B8C7");
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
