using System;
namespace Toolbox.Droid.Helpers
{
    public static class PixelHelper
    {
        public static int PixelsToDp(float pixelValue, float density)
        {
            return (int)((pixelValue) / density);
        }

        public static int DpToPixels(float dp, float density)
        {
            return (int)(dp * density);
        }
    }
}
