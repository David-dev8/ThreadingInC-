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

        public SimulationDataViewModel(NavigationService navigationService, Simulation simulation) : base(navigationService)
        {
            DataLoaded = false;
            Simulation = simulation;
            GoToHomeCommand = new RelayCommand(() => SelectHomeAsCurrentPage());

            if(Simulation.PopulationManager.SpeciesCount.Count == 0)
            {
                InitializeData();
            }
        }

        private async Task InitializeData()
        {
            try
            {
                await TryLoad();
                DataLoaded = true;
            }
            catch(Exception ex)
            {
                _navigationService.Error = new ErrorMessage("Data could not be fetched");
                SelectHomeAsCurrentPage();
            }
        }

        private async Task TryLoad()
        {
            ISimulationReader simulationReader = new DatabaseSimulationReader();
            Simulation = await simulationReader.ReadFullDetails(Simulation);
            IDictionary<Species, IDictionary<DateTime, int>> speciesCount = Simulation.PopulationManager.GetSpeciesCountPerSpecies();
            SpeciesCount = speciesCount.ToDictionary(currentSpeciesCount => currentSpeciesCount.Key, currentSpeciesCount => GetDataPoints(currentSpeciesCount.Value));
            ShannonWeaverIndices = GetDataPoints(Simulation.PopulationManager.GetShannonWeaverData());
            DominatingSpecies = Simulation.PopulationManager.GetDominatingSpecies().Take(3);
            AffectedStatistics = Simulation.MutationManager.Analyze();
        }

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
