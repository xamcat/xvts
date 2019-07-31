using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Android.App;
using Faucet.ViewModels;
using Android.Support.V7.App;

namespace Faucet.Droid
{
    public abstract class BaseActivity<T> : AppCompatActivity where T : BaseViewModel, new()
    {
        public T ViewModel
        {
            get;
            set;
        }

        protected BaseActivity()
        {
            ViewModel = new T();
        }

        protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Task.Run(async () =>
            {
                try
                {
                    await ViewModel.InitAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            });
        }
    }
}
