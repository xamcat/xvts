using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using MCWeather.Forms.Controls;
using MCWeather.Common.iOS.Views;

[assembly: ExportRenderer(typeof(Thermometer),
                          typeof(MCWeather.Forms.iOS.Renderers.ThermometerRenderer))]

namespace MCWeather.Forms.iOS.Renderers
{
    public class ThermometerRenderer : ViewRenderer<Thermometer, ThermometerView>
    {
        ThermometerView _thermometer;

        protected override void OnElementChanged(ElementChangedEventArgs<Thermometer> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                _thermometer = new ThermometerView();
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
                // Subscribe to events
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

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var bounds = Bounds;

            _thermometer.Frame = bounds;
        }

        void OnTemperatureChanged()
        {
            Element.Temperature = _thermometer.Temperature.GetValueOrDefault(0);
        }
    }
}
