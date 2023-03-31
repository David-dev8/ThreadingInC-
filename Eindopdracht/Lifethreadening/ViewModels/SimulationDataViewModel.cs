using Lifethreadening.Base;
using Lifethreadening.DataAccess;
using Lifethreadening.DataAccess.Database;
using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lifethreadening.ViewModels
{
    /// <summary>
    /// This viewmodel is used for the simulation data view
    /// </summary>
    public class SimulationDataViewModel : BaseViewModel
    {
        private static readonly int maxAmountOfDatapoints = 100;
        
        public ICommand GoToHomeCommand { get; set; }

        public Simulation Simulation { get; set; }

        private bool _dataLoaded;

        public bool DataLoaded
        {
            get
            {
                return _dataLoaded;
            }
            set
            {
                _dataLoaded = value;
                NotifyPropertyChanged();
            }
        }

        private IDictionary<StatisticInfo, int> _affectedStatistics;

        public IDictionary<StatisticInfo, int> AffectedStatistics
        {
            get
            {
                return _affectedStatistics;
            }
            set
            {
                _affectedStatistics = value;
                NotifyPropertyChanged();
            }
        }

        private IDictionary<Species, IDictionary<DateTime, int>> _speciesCount;

        public IDictionary<Species, IDictionary<DateTime, int>> SpeciesCount
        {
            get
            {
                return _speciesCount;
            }
            set
            {
                _speciesCount = value;
                NotifyPropertyChanged();
            }
        }

        private IDictionary<DateTime, double> _shannonWeaverIndices;

        public IDictionary<DateTime, double> ShannonWeaverIndices
        {
            get
            {
                return _shannonWeaverIndices;
            }
            set
            {
                _shannonWeaverIndices = value;
                NotifyPropertyChanged();
            }
        }

        private IEnumerable<Rank> _dominatingSpecies;

        public IEnumerable<Rank> DominatingSpecies
        {
            get
            {
                return _dominatingSpecies;
            }
            set
            {
                _dominatingSpecies = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Creates a new simulation data view model
        /// </summary>
        /// <param name="navigationService">The navigation service to use when navigating</param>
        /// <param name="simulation">The simulation that will get its data displayed</param>
        public SimulationDataViewModel(NavigationService navigationService, Simulation simulation) : base(navigationService)
        {
            DataLoaded = false;
            Simulation = simulation;
            GoToHomeCommand = new RelayCommand(() => SelectHomeAsCurrentPage());
            
            InitializeData();
        }

        private async Task InitializeData()
        {
            try
            {
                await TryLoad();
                DataLoaded = true;
            }
            catch(Exception)
            {
                _navigationService.Error = new ErrorMessage("Data could not be fetched");
                SelectHomeAsCurrentPage();
            }
        }

        private async Task TryLoad()
        {
            if(Simulation.PopulationManager.SpeciesCount.Count == 0)
            {
                ISimulationReader simulationReader = new DatabaseSimulationReader();
                Simulation = await simulationReader.ReadFullDetails(Simulation);
            }
            IDictionary<Species, IDictionary<DateTime, int>> speciesCount = Simulation.PopulationManager.GetSpeciesCountPerSpecies();
            SpeciesCount = speciesCount.ToDictionary(currentSpeciesCount => currentSpeciesCount.Key, currentSpeciesCount => GetDataPoints(currentSpeciesCount.Value));
            ShannonWeaverIndices = GetDataPoints(Simulation.PopulationManager.GetShannonWeaverData());
            DominatingSpecies = Simulation.PopulationManager.GetDominatingSpecies().Take(3);
            AffectedStatistics = Simulation.MutationManager.Analyze();
        }

        /// <summary>
        /// This function is used to navigate to the home view
        /// </summary>
        private void SelectHomeAsCurrentPage()
        {
            _navigationService.CurrentViewModel = new HomeViewModel(_navigationService);
        }

        private IDictionary<K, V> GetDataPoints<K, V>(IDictionary<K, V> dictionary)
        {
            int gap = (int)Math.Ceiling((double)dictionary.Count / maxAmountOfDatapoints);
            return dictionary.Where((data, index) => index % gap == 0).ToDictionary(data => data.Key, data => data.Value);
        }
    }
}
