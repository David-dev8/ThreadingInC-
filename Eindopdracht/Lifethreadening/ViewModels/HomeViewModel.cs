using Lifethreadening.Base;
using Lifethreadening.DataAccess.Database;
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
        private const int AMOUNT_OF_SLOTS = 3;
        private KeyValuePair<string, Simulation> _selectedSlot;

        public ICommand CreateNewGameCommand { get; set; }
        public KeyValuePair<string, Simulation> SelectedSlot
        {
            get
            {
                return _selectedSlot;
            }
            set
            {
                _selectedSlot = value;
                NotifyPropertyChanged();
            }
        }
        public ICommand GoToStatisticsCommand { get; set; }
        public ICommand GoToCustomSpeciesCommand { get; set; }
        public IDictionary<string, Simulation> Slots { get; set; }


        private Simulation _selectedPastGame;
        public Simulation SelectedPastGame
        {
            get 
            {
                return _selectedPastGame;
            }
            set 
            {
                _selectedPastGame = value;
                NavigateToStats(value);
            }
        }

        public List<Simulation> PastGames 
        {
            get 
            {
                List<Simulation> completedSims = new DatabaseSimulationReader().ReadAll().Where((s) => s.Filename == "").ToList(); ;
                return completedSims;
            }
        }

        public HomeViewModel(NavigationService navigationService) : base(navigationService)
        {
            CreateNewGameCommand = new RelayCommand(CreateNewGame);
            GoToCustomSpeciesCommand = new RelayCommand(NavigateToCustomSpiecies);

            Slots = new Dictionary<string, Simulation>();
            for(int i = 0; i < AMOUNT_OF_SLOTS; i++)
            {
                Slots.Add(i.ToString(), null);
            }
            SelectedSlot = Slots.First();
        }

        private void CreateNewGame()
        {
            _navigationService.CurrentViewModel = new EcosystemSelectViewModel(_navigationService);
        }

        public void NavigateToStats(Simulation simulation)
        {
            _navigationService.CurrentViewModel = new SimulationDataViewModel(_navigationService, simulation);
        }

        public void NavigateToCustomSpiecies()
        {
            _navigationService.CurrentViewModel = new CustomSpeciesViewModel(_navigationService);
        }
    }
}
