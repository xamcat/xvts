using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MCWeather.Common.Models;
using MCWeather.Common.Services.Interfaces;
using Toolbox.Portable.Services;
using Toolbox.Portable.Infrastructure;

namespace MCWeather.Common.Services
{
    public class ImageService : BaseHttpService, IImageService
    {
        public ImageService()
            : base("https://xvtsfapktnxnh4rzvvie.azurewebsites.net/api/functions/Image/")
        {
        }

        public async Task<string> GetBackgroundImageAsync(string city, string weather)
        {
            var searchWeather = weather.Replace(" ", "+");

            var remoteImage = await GetAsync<CityBackground>($"BackgroundByCityAndWeather?city={city}&weather={searchWeather}&height=1920").ConfigureAwait(false);
            var cityBackground = new CityBackground
            {
                Name = city,
                WeatherOverview = searchWeather,
                ImageUrl = remoteImage.ImageUrl
            };

            return await WebImageCache.RetrieveImage(cityBackground.ImageUrl, $"{city}-{searchWeather}");
        }
    }
}
