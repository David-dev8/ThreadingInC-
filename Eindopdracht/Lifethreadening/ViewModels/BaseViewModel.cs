using Lifethreadening.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.ViewModels
{
    /// <summary>
    /// This is the base view model that all other view models are based on
    /// </summary>
    public class BaseViewModel : Observable
    {
        protected NavigationService _navigationService;

        /// <summary>
        /// Creates a new view model
        /// </summary>
        /// <param name="navigationService">The navigation service to use when navigating</param>
        public BaseViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        /// <summary>
        /// this function disposes the view model
        /// </summary>
        public virtual void Dispose()
        {
        }
    }
}
