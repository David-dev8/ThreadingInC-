using Lifethreadening.Base;
using Lifethreadening.DataAccess.Database;
using Lifethreadening.ExtensionMethods;
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
        private IList<Simulation> _games;

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
                if(_selectedSlot.Value != null)
                {
                    OpenSimulation(_selectedSlot.Value);
                }
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

        public List<Simulation> PastGames { get; set; }

        public HomeViewModel(NavigationService navigationService) : base(navigationService)
        {
            _games = new DatabaseSimulationReader().ReadAll().ToList();

            PastGames = _games.Where(s => !CanBeOpened(s)).ToList();
            Queue<string> slotNames = new Queue<string>(new string[] { "A", "B", "C" });
            Slots = _games.Where(CanBeOpened).PadUntilLength(3, null).Take(3).ToDictionary(simulation => slotNames.Dequeue(), simulation => simulation);

            CreateNewGameCommand = new RelayCommand(CreateNewGame, (_) => Slots.Any(s => s.Value == null));
            GoToCustomSpeciesCommand = new RelayCommand(NavigateToCustomSpiecies);
        }

        private void CreateNewGame()
        {
            _navigationService.CurrentViewModel = new EcosystemSelectViewModel(_navigationService);
        }

        private void OpenSimulation(Simulation simulation)
        {
            _navigationService.CurrentViewModel = new SimulationViewModel(_navigationService, simulation);
        }

        public void NavigateToStats(Simulation simulation)
        {
            _navigationService.CurrentViewModel = new SimulationDataViewModel(_navigationService, simulation);
        }

        public void NavigateToCustomSpiecies()
        {
            _navigationService.CurrentViewModel = new CustomSpeciesViewModel(_navigationService);
        }

        private bool CanBeOpened(Simulation simulation)
        {
            return simulation.Filename != "";
        }
    }
}
