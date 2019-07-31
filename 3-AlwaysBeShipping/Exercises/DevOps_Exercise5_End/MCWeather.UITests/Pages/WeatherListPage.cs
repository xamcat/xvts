using System;
using NUnit.Framework;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;
using Xamarin.UITest.Configuration;

namespace MCWeather.UITests
{
    public class WeatherListPage : BasePage
    {
        readonly Query addLocationButton;
        readonly Func<string, Query> locationListItem;
        readonly Query addCityAlert;
        readonly Query enterCityField;
        readonly Query alertOKButton;
        readonly Query listView;

        protected override PlatformQuery Trait => new PlatformQuery
        {
            Android = x => x.Marked("WeatherListView"),
            iOS = x => x.Marked("WeatherListView")
        };

        public WeatherListPage()
        {
            addLocationButton = x => x.Property("text").Contains("Add");
            addCityAlert = x => x.Marked("Enter city name");
            alertOKButton = x => x.Marked("Ok");
            listView = x => x.Marked("WeatherListView");

            if (OnAndroid)
            {
                locationListItem = name => x => x.Class("ListView").Descendant().Text(name);
                enterCityField = x => x.Class("EditText");
            }

            if (OniOS)
            {
                locationListItem = name => x => x.Class("UITableView").Descendant().Text(name);
                enterCityField = x => x.Class("UITextField");
            }
        }

        public void SelectCity(string cityName)
        {
            app.WaitForElement(locationListItem(cityName));
            app.Screenshot($"Tapping on: {cityName}");
            app.Tap(locationListItem(cityName));
        }

        public WeatherListPage VerifyCityAdded(string cityName)
        {
            app.WaitForElement(locationListItem(cityName));
            app.Screenshot($"{cityName} : City Added");

            return this;
        }

        public WeatherListPage AddNewCity(string cityName)
        {
            app.Tap(addLocationButton);
            app.WaitForElement(addCityAlert);
            app.EnterText(enterCityField, cityName);
            app.DismissKeyboard();
            app.Tap(alertOKButton);
            app.Screenshot($"City Name: {cityName} Entered");

            return this;
        }

        public WeatherListPage AddFavoriteBackdoor(string cityName)
        {
            string result = string.Empty;

            if (OnAndroid)
                result = app.Invoke("addFavorite", cityName).ToString();

            if (OniOS)
                result = app.Invoke("addFavorite:", cityName).ToString();

            Assert.AreEqual("true", result.ToLower(), "Invoking backdoor did not return true");

            var rect = app.Query(listView)[0].Rect;
            app.DragCoordinates(rect.CenterX, rect.Y + 20, rect.CenterX, rect.Y + rect.Height);

            return this;
        }
    }
}
