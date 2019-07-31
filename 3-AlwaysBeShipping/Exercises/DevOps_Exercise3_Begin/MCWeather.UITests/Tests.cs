using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace MCWeather.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void AppLaunches()
        {
            app.Screenshot("First screen.");
        }

        // Exercise 1 answer
        [Test]
        public void WeatherLocationTest()
        {
            app.WaitForElement(x => x.Text("Bellevue"));
            app.Screenshot("App Launched on City List Page");

            app.Tap(x => x.Text("San Francisco"));
            app.Screenshot("Tapped on city San Francisco");

            app.WaitForElement(x => x.Marked("currentTemperatureLabel"));
            app.Screenshot("On Weather Details Page");
        }

        // Exercise 1 bonus answer
        [Test]
        public void AddFavCityTest()
        {
            app.WaitForElement(x => x.Text("Bellevue"));
            app.Screenshot("App Launched on City List Page");

            app.Tap(x => x.Property("text").Contains("Add"));
            app.Screenshot("Tapped on Add Button");

            if (platform == Platform.Android)
            {
                app.WaitForElement(x => x.Class("EditText"));
                app.EnterText(x => x.Class("EditText"), "Boston");
            }

            if (platform == Platform.iOS)
            {
                app.WaitForElement(x => x.Class("UITextField"));
                app.Tap(x => x.Class("UITextField"));
                app.EnterText(x => x.Class("UITextField"), "Boston");
            }

            app.DismissKeyboard();
            app.Screenshot("Boston: City Entered");

            app.Tap(x => x.Marked("Ok"));

            app.WaitForElement(x => x.Text("Boston"));
            app.Screenshot("City Added to List");
        }

        // Exercise 2 answer
        [Test]
        public void InvokeBackDoor()
        {
            app.WaitForElement(x => x.Text("Bellevue"));
            app.Screenshot("App Launched on City List Page");

            string result = string.Empty;

            if (platform == Platform.Android)
                result = app.Invoke("addFavorite", "New York").ToString();

            if (platform == Platform.iOS)
                result = app.Invoke("addFavorite:", "New York").ToString();

            var rect = app.Query(x => x.Marked("WeatherListView"))[0].Rect;
            app.DragCoordinates(rect.CenterX, rect.Y + 20, rect.CenterX, rect.Y + rect.Height);

            Assert.AreEqual("true", result.ToLower(), "Invoking backdoor did not return true");

            app.WaitForElement(x => x.Text("New York"));
            app.Screenshot("Backdoor Successful!");
        }
    }
}
