using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using MCWeather.Forms.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName(MCWeatherEffects.ResolutionGroupName)]
[assembly: ExportEffect(typeof(MCWeather.Forms.Droid.Effects.AndroidShadowEffect), nameof(ShadowEffect))]
namespace MCWeather.Forms.Droid.Effects
{
    public class AndroidShadowEffect: PlatformEffect
    {
		protected override void OnAttached()
		{
			try
			{
				var control = Control as Android.Widget.TextView;
                var shadowEffect = (ShadowEffect)Element.Effects.FirstOrDefault(e => e is ShadowEffect);
				var color = shadowEffect.Color.ToAndroid();
                control.SetShadowLayer((float)ShadowEffect.Radius, (float)ShadowEffect.DistanceX, (float)ShadowEffect.DistanceY, color);
			}
			catch (Exception ex)
			{
                Debug.WriteLine($"Cannot set property on attached control. Error: {ex.Message}");
			}
		}

		protected override void OnDetached()
		{
		}
    }
}