using System;
using UIKit;
using System.Collections.Generic;
using Toolbox.Portable.Mvvm;
using System.Linq;

namespace MCWeather.iOS.Controllers
{
    public class BaseViewController : UIViewController
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

        // Needed to prevent the linker from removing the Title property, which we bind to
        [Foundation.Preserve]
        private void PreserveHack()
        {
            Title = Title;
        }
    }
}
