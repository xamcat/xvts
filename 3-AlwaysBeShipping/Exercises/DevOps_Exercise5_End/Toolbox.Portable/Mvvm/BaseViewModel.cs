using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Toolbox.Portable.Mvvm
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (RaiseAndUpdate(ref isBusy, value))
                    Raise(nameof(IsNotBusy));
            }
        }

        public bool IsNotBusy => !IsBusy;

        //List<KeyValuePair<string, List<Action>>> PropertyWatchers = new List<KeyValuePair<string, List<Action>>>();

        public virtual Task InitAsync() => Task.FromResult(true);

        protected bool RaiseAndUpdate<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            Raise(propertyName);

            return true;
        }

        protected void Raise(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //    var watchers = PropertyWatchers.FirstOrDefault(pw => pw.Key == propertyName);
        //    if (watchers.Equals(default(KeyValuePair<string, List<Action>>)))
        //        return;

        //    foreach (Action watcher in watchers.Value)
        //    {
        //        watcher();
        //    }
        //}

        //public void WatchProperty(string propertyName, Action action)
        //{
        //    if (PropertyWatchers.All(pw => pw.Key != propertyName))
        //    {
        //        PropertyWatchers.Add(new KeyValuePair<string, List<Action>>(propertyName, new List<Action>()));
        //    }

        //    PropertyWatchers.First(pw => pw.Key == propertyName).Value.Add(action);
        //}

        //public void ClearWatchers()
        //{
        //    PropertyWatchers.Clear();
        //}
    }
}