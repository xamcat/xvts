using System;
using MCWeather.Forms.Controls;
using MCWeather.Common.Droid.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;

[assembly: ExportRenderer(typeof(Thermometer),
                          typeof(MCWeather.Forms.Droid.Renderers.ThermometerRenderer))]

namespace MCWeather.Forms.Droid.Renderers
{
    public class ThermometerRenderer : ViewRenderer<Thermometer, ThermometerView>
    {
        ThermometerView _thermometer;

        public ThermometerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Thermometer> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                _thermometer = new ThermometerView(this.Context);
                SetNativeControl(_thermometer);
            }

            if (e.OldElement != null)
            {
                // Clean up any resources
                _thermometer.TemperatureChanged -= OnTemperatureChanged;
                _thermometer.Dispose();
            }

            if (e.NewElement != null)
            {
                // Subscribe to any events
                _thermometer.TemperatureChanged += OnTemperatureChanged;
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(Thermometer.IsImperial))
                _thermometer.IsImperial = Element.IsImperial;

            if (e.PropertyName == nameof(Thermometer.Temperature))
                _thermometer.Temperature = Element.Temperature;
        }

        void OnTemperatureChanged()
        {
            Element.Temperature = _thermometer.Temperature.GetValueOrDefault(0);
        }
    }
}
