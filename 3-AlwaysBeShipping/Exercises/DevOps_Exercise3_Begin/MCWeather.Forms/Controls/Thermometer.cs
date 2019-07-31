using System;
using Xamarin.Forms;

namespace MCWeather.Forms.Controls
{
    public class Thermometer : View
    {
        public static readonly BindableProperty TemperatureProperty = BindableProperty.Create(
            nameof(Temperature),
            typeof(int),
            typeof(Thermometer),
            0,
            BindingMode.TwoWay);

        public static readonly BindableProperty IsImperialProperty = BindableProperty.Create(
            nameof(IsImperial),
            typeof(bool),
            typeof(Thermometer),
            false,
            BindingMode.TwoWay);

        public int Temperature
        {
            get { return (int)GetValue(TemperatureProperty); }
            set { SetValue(TemperatureProperty, value); }
        }

        public bool IsImperial
        {
            get { return (bool)GetValue(IsImperialProperty); }
            set { SetValue(IsImperialProperty, value); }
        }
    }
}
