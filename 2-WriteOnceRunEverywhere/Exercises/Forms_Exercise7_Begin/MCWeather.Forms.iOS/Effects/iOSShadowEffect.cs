using System;
using System.Diagnostics;
using System.Linq;
using CoreGraphics;
using MCWeather.Forms.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName(MCWeatherEffects.ResolutionGroupName)]
[assembly: ExportEffect(typeof(MCWeather.Forms.iOS.Effects.iOSShadowEffect), nameof(ShadowEffect))]
namespace MCWeather.Forms.iOS.Effects
{
    public class iOSShadowEffect : PlatformEffect
    {
		protected override void OnAttached()
		{
			try
			{
                var shadowEffect = (ShadowEffect)Element.Effects.FirstOrDefault(e => e is ShadowEffect);
				var color = shadowEffect.Color.ToCGColor();
				Control.Layer.ShadowColor = color;
				Control.Layer.CornerRadius = (nfloat)ShadowEffect.Radius;
				Control.Layer.ShadowOffset = new CGSize((double)ShadowEffect.DistanceX, (double)ShadowEffect.DistanceY);
				Control.Layer.ShadowOpacity = 1.0f;
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