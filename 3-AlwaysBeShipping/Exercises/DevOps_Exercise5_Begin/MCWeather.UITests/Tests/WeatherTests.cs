using System;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Configuration;

namespace MCWeather.UITests
{
    public class WeatherTests : BaseTestFixture
    {
        public WeatherTests(Platform platform)
            : base(platform)
        {
        }

        [Test]
        public void WeatherLocationTest()
        {
            new WeatherListPage()
                .SelectCity("San Francisco");

            new WeatherDetailPage()
                .VerifyWeatherVisible();
        }

        [Test]
        public void AddFavCityTest()
        {
            var cityName = "Boston";

            new WeatherListPage()
                .AddNewCity(cityName)
                .VerifyCityAdded(cityName);
        }

        [Test]
        public void InvokeBackDoor()
        {
            new WeatherListPage()
                .AddFavoriteBackdoor("New York")
                .VerifyCityAdded("New York");
        }

        [TestCase("Boston")]
        [TestCase("Chicago")]
        public void VerifyNewCityTest(string cityName)
        {
            new WeatherListPage()
                .AddNewCity(cityName)
                .SelectCity(cityName);

            new WeatherDetailPage()
                .VerifyWeatherVisible();
        }
    }
}
