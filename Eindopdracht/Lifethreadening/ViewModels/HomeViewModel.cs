using Lifethreadening.Base;
using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lifethreadening.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public ICommand OpenGameA { get; set; }
        public ICommand OpenGameB { get; set; }
        public ICommand OpenGameC { get; set; }
        public ICommand GoToStatistics { get; set; }
        public ICommand GoToCustomSpiecies { get; set; }

        public HomeViewModel(NavigationService navigationService) : base(navigationService)
        {

            OpenGameA = new RelayCommand(() => StartGame(0));
            OpenGameB = new RelayCommand(() => StartGame(1));
            OpenGameC = new RelayCommand(() => StartGame(2));

            GoToStatistics = new RelayCommand(NavigateToStats);
            GoToCustomSpiecies = new RelayCommand(NavigateToCustomSpiecies);

        }

        public void StartGame(int slot) 
        {
            //TODO safeslots
            //Simulation simul = new Simulation();

            //_navigationService.CurrentViewModel = new SimulationViewModel(_navigationService, simul);
        }

        public void NavigateToStats()
        {
            _navigationService.CurrentViewModel = new SimulationDataViewModel(_navigationService,null);
        }

        public void NavigateToCustomSpiecies()
        {
            _navigationService.CurrentViewModel = new CustomSpeciesViewModel(_navigationService);
        }
    }
}
