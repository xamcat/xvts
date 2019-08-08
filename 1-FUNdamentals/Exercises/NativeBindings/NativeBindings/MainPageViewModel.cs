using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NativeBindings
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged<T>(ref T property, T propertyValue, [CallerMemberName] string propertyName = null)
        {
            property = propertyValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Random _random = new Random();

        private float _progress = 0f;
        public float ProgressValue
        {
            get => _progress;
            set => RaisePropertyChanged(ref _progress, value);
        }

        public MainPageViewModel()
        {
            Task.Run(StartInfiniteProgress).ConfigureAwait(false);
        }

        private async Task StartInfiniteProgress()
        {
            while (true)
            {
                if (ProgressValue > 1f)
                {
                    await Task.Delay(400);
                    ProgressValue = 0f;
                    await Task.Delay(2000);
                }
                ProgressValue += 0.05f * (float)_random.NextDouble();
                await Task.Delay(_random.Next(200,600));
            }
        }
    }
}
