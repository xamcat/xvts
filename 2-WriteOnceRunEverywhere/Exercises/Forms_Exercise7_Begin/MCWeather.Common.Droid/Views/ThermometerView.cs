using System;
using Android.Views;
using Android.Content;
using Android.Widget;
using Android.Graphics.Drawables;
using Android.Media;
using Android.Support.V4.Content.Res;
using Android.Support.V4.Content;
using Android.Views.Animations;
using Android.App;
using Android.Util;
using Android.Runtime;

namespace MCWeather.Common.Droid.Views
{
    public class ThermometerView : ViewGroup
    {
        ImageView _backgroundImage;
        ImageView _needleImage;

        private bool _isImperial = false;
        private int? _temperature = null;

        public Action TemperatureChanged;

        public bool IsImperial
        {
            get { return _isImperial; }
            set
            {
                if (_isImperial != value)
                {
                    _isImperial = value;
                    UpdateUnits();
                }
            }
        }
        public int? Temperature
        {
            get { return _temperature; }
            set
            {
                if (_temperature != value)
                {
                    _temperature = value;
                    UpdateTemperature();
                }
            }
        }

        public ThermometerView(Context context)
            : base(context)
        {
            Initialize();
        }

        public ThermometerView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            Initialize();
        }

        public ThermometerView(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
            Initialize();
        }

        public ThermometerView(Context context, IAttributeSet attrs, int defStyle, int defStyleRes)
            : base(context, attrs, defStyle, defStyleRes)
        {
            Initialize();
        }

        public ThermometerView(IntPtr handle, JniHandleOwnership owner)
            : base(handle, owner)
        {
            Initialize();
        }

        void Initialize()
        {
            var context = Application.Context;

            _backgroundImage = new ImageView(context);
            _backgroundImage.Clickable = true;
            _backgroundImage.SetImageDrawable(GetBackgroundImageList());

            AddView(_backgroundImage);

            _needleImage = new ImageView(context);
            _needleImage.SetImageResource(Resource.Drawable.needle);

            AddView(_needleImage);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            if (!changed) return;

            _backgroundImage.Layout(l + PaddingLeft, t + PaddingTop, r - PaddingRight, b - PaddingBottom);
            _needleImage.Layout(l + PaddingLeft, t + PaddingTop, r - PaddingRight, b - PaddingBottom);
        }

        StateListDrawable GetBackgroundImageList()
        {
            var backgroundImageState = new StateListDrawable();

            backgroundImageState.AddState(new int[] { global::Android.Resource.Attribute.StateChecked }, ContextCompat.GetDrawable(this.Context, Resource.Drawable.celsius));
            backgroundImageState.AddState(new int[] { }, ContextCompat.GetDrawable(this.Context, Resource.Drawable.fahrenheit));

            return backgroundImageState;
        }

        private float angle = 0.0f;

        void UpdateTemperature()
        {
            var rotation = (IsImperial ? 2 : 1) * Temperature;

            var animSet = new AnimationSet(true);
            animSet.FillAfter = true;
            animSet.FillEnabled = true;

            var animRotate = new RotateAnimation(angle,
                                                 rotation.GetValueOrDefault(0),
                                                 Dimension.RelativeToSelf, 0.5f,
                                                 Dimension.RelativeToSelf, 0.5f);

            animRotate.Duration = 400;
            animRotate.FillAfter = true;
            animSet.AddAnimation(animRotate);

            _needleImage.StartAnimation(animSet);

            angle = rotation.GetValueOrDefault(0);
        }

        void UpdateUnits()
        {
            var state = IsImperial ? new int[] { global::Android.Resource.Attribute.StateChecked } : new int[] { };

            _backgroundImage.SetImageState(state, false);

            Temperature = IsImperial ? Temperature / 2 : Temperature * 2;
            TemperatureChanged?.Invoke();
        }
    }
}
