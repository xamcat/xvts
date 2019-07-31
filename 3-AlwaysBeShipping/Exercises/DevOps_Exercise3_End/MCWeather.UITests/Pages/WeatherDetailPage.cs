using System;
using NUnit.Framework;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace MCWeather.UITests
{
    public class WeatherDetailPage : BasePage
    {
        readonly Query thermometerView;
        readonly Query descriptionLabel;
        readonly Query currentTemperatureLabel;
        readonly Query highLowTemperatureLabel;
        readonly Query backButton;

        protected override PlatformQuery Trait => new PlatformQuery
        {
            Android = x => x.Marked("WeatherDetailView"),
            iOS = x => x.Marked("WeatherDetailView")
        };

        public WeatherDetailPage()
        {
            thermometerView = x => x.Class("ThermometerView");
            descriptionLabel = x => x.Marked("descriptionLabel");
            currentTemperatureLabel = x => x.Marked("currentTemperatureLabel");
            highLowTemperatureLabel = x => x.Marked("highAndLowTemperatureLabel");

            if (OniOS)
            {
                backButton = x => x.Marked("Back");
            }
            else
            {
                backButton = x => x.Class("AppCompatImageButton");
            }
        }


        public WeatherDetailPage VerifyWeatherVisible()
        {
            app.WaitForElement(descriptionLabel);
            Assert.NotNull(app.Query(thermometerView), "Thermometer View not Visible");
            Assert.NotNull(app.Query(descriptionLabel)[0].Text, "Description Text not Visible");
            Assert.NotNull(app.Query(currentTemperatureLabel)[0].Text, "Current Temperature Text not Visible");
            Assert.NotNull(app.Query(highLowTemperatureLabel)[0].Text, "High/Low Temperature Label not Visible");

            app.Screenshot("Weather verified as visible");

            return this;
        }

        public void GoBack()
        {
            app.Tap(backButton);
        }
    }
}
