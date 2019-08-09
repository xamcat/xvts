using System;
using Xamarin.Forms;

namespace NativeBindings
{
    public class ProgressView : View
    {
        public static readonly BindableProperty IsDisplayedProperty = BindableProperty.Create(nameof(IsDisplayed),
            typeof(bool),
            typeof(ProgressView),
            default(bool));

        public bool IsDisplayed
        {
            get { return (bool)GetValue(IsDisplayedProperty); }
            set { SetValue(IsDisplayedProperty, value); }
        }


        public static readonly BindableProperty ProgressValueProperty = BindableProperty.Create(nameof(ProgressValue),
            typeof(float),
            typeof(ProgressView),
            default(float));

        public float ProgressValue
        {
            get { return (float)GetValue(ProgressValueProperty); }
            set { SetValue(ProgressValueProperty, value); }
        }
    }
}
