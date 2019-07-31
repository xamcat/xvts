using System;
using System.Collections.Generic;
using System.Linq;
using Toolbox.Portable.Mvvm;
using Android.App;
using Android.Support.V7.App;

namespace MCWeather.Droid.Activities
{
    public class BaseActivity : AppCompatActivity
    {
        List<Binding> _bindings = new List<Binding>();

        protected virtual void AddBinding(Binding binding)
        {
            _bindings.Add(binding);
        }

        protected virtual void RemoveBindings()
        {
            while (_bindings.Any())
            {
                var binding = _bindings.FirstOrDefault();
                binding?.Dispose();

                _bindings.Remove(binding);
            }
        }
    }
}
