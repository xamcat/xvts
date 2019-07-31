using System;
using Faucet.ViewModels;
using UIKit;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Faucet.iOS.ViewControllers
{
    public abstract class BaseViewController<T> : UIViewController where T : BaseViewModel, new()
    {
        public T ViewModel
        {
            get;
            set;
        }

        protected BaseViewController()
        {
            ViewModel = new T();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.BackgroundColor = UIColor.White;

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

