using Lifethreadening.Base;
using Lifethreadening.DataAccess;
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
    /// <summary>
    /// This is the viewmodel for the home view
    /// </summary>
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

        /// <summary>
        /// Creates a new home view model
        /// </summary>
        /// <param name="navigationService">The navigation service to be used when navigating</param>
        public HomeViewModel(NavigationService navigationService) : base(navigationService)
        {
            InitializeSlots();

            CreateNewGameCommand = new RelayCommand(CreateNewGame, (_) => Slots != null && Slots.Any(s => s.Value == null));
            GoToCustomSpeciesCommand = new RelayCommand(NavigateToCustomSpiecies);
        }

        private void InitializeSlots()
        {
            try
            {
                _games = new DatabaseSimulationReader().ReadAll().ToList();
                PastGames = _games.Where(s => !CanBeOpened(s)).ToList();
                Queue<string> slotNames = new Queue<string>(new string[] { "A", "B", "C" });
                Slots = _games.Where(CanBeOpened).PadUntilLength(3, null).Take(AMOUNT_OF_SLOTS).ToDictionary(simulation => slotNames.Dequeue(), simulation => simulation);
            }
            catch (Exception) 
            {
                _navigationService.Error = new ErrorMessage("The games could not be loaded.", "Load failed");
            }
        }

        /// <summary>
        /// This method creates a new game / simulation
        /// </summary>
        private void CreateNewGame()
        {
            _navigationService.CurrentViewModel = new EcosystemSelectViewModel(_navigationService, CreateSimulationName());
        }

        private string CreateSimulationName()
        {
            return "Saveslot" + Slots.First(s => s.Value == null).Key + DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        private void OpenSimulation(Simulation simulation)
        {
            _navigationService.CurrentViewModel = new SimulationViewModel(_navigationService, simulation);
        }

        /// <summary>
        /// This method navigates the player to the statistics and data of a previously finished game
        /// </summary>
        /// <param name="simulation">The finished simulation to get the data for</param>
        public void NavigateToStats(Simulation simulation)
        {
            _navigationService.CurrentViewModel = new SimulationDataViewModel(_navigationService, simulation);
        }

        /// <summary>
        /// This method is used to navigate to the create a new species view
        /// </summary>
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
