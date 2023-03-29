using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Lifethreadening.Base
{
    public abstract class Observable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            // TODO exception?
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            });
            #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            OnPropertyChanged(propertyName);
        }
    }
}
