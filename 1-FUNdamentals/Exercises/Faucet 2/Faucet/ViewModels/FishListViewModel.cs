using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Faucet.Models;

namespace Faucet.ViewModels
{
    public class FishListViewModel : BaseViewModel
    {
        private ObservableCollection<Fish> _fish = new ObservableCollection<Fish>();

        public FishListViewModel()
        {
            for (int i = 0; i < 1000; i++)
            {
                Fish.Add(new Fish { Name = "Trout", ImageSource = "trout" });
                Fish.Add(new Fish { Name = "Sea Bass", ImageSource = "seabass" });
                Fish.Add(new Fish { Name = "Salmon", ImageSource = "salmon" });
            }
        }

        public Action ReloadAction { get; set; }

        public ObservableCollection<Fish> Fish
        {
            get { return _fish; }
            set { RaiseAndUpdate(ref _fish, value); }
        }
    }
}
