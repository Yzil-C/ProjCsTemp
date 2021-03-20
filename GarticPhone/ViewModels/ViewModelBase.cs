using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GarticPhone.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {

        public ViewModelLocator Locator => App.Current.Resources["Locator"] as ViewModelLocator;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
