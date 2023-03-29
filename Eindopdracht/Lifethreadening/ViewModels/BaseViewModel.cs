using Lifethreadening.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.ViewModels
{

    public class BaseViewModel : Observable, IDisposable
    {
        protected NavigationService _navigationService;

        public BaseViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public virtual void Dispose()
        {
        }
    }
}
