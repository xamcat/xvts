using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace MCWeather.Forms.Effects
{
    [Preserve(AllMembers = true)]
    public class ShadowEffect : RoutingEffect
    {
        public static readonly double Radius = 10;
        public static readonly double DistanceX = 2;
        public static readonly double DistanceY = 2;
        public Color Color { get; set; }

        public ShadowEffect() : base($"{MCWeatherEffects.ResolutionGroupName}.{nameof(ShadowEffect)}")
        {
        }
    }
}