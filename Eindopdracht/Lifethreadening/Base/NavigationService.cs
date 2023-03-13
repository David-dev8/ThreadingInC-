using Lifethreadening.ViewModels;
using Lifethreadening.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Lifethreadening.Base
{
    public class NavigationService
    {
        private Frame _frame;
        private BaseViewModel _currentViewModel;
        private readonly Dictionary<Type, Type> viewMapping = new Dictionary<Type, Type>() 
        {
            { typeof(CustomSpeciesViewModel), typeof(CustomSpeciesView) },
            { typeof(EcosystemSelectViewModel), typeof(EcosystemSelectView) },
            { typeof(HomeViewModel), typeof(HomeView) },
            { typeof(SimulationDataViewModel), typeof(SimulationDataView) },
            { typeof(SimulationViewModel), typeof(SimulationView) }
        };

        public BaseViewModel CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                _currentViewModel = value;
                _frame.Navigate(viewMapping[_currentViewModel.GetType()]);
                _frame.DataContext = _currentViewModel;
            }
        }

        public NavigationService(Frame frame)
        {
            _frame = frame;
        }
    }
}
