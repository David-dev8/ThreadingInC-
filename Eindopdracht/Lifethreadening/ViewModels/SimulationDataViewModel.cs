using Lifethreadening.Base;
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
        private static readonly int MAX_AMOUNT_OF_DATAPOINTS = 100;
        
        public ICommand GoToHomeCommand { get; set; }

        public Simulation Simulation { get; set; }

        public IDictionary<StatisticInfo, int> AffectedStatistics
        {
            get
            {
                return new Dictionary<StatisticInfo, int>();
            }
        }

        public IDictionary<Species, IDictionary<DateTime, int>> SpeciesCount
        {
            get
            {
                IDictionary<Species, IDictionary<DateTime, int>> speciesCount = Simulation.PopulationManager.GetSpeciesCountPerSpecies();
                return speciesCount.ToDictionary(currentSpeciesCount => currentSpeciesCount.Key, currentSpeciesCount => GetDataPoints(currentSpeciesCount.Value));
            }
        }

        public IDictionary<DateTime, double> ShannonWeaverIndices
        {
            get
            {
                return GetDataPoints(Simulation.PopulationManager.GetShannonWeaverData());
            }
        }

        public IEnumerable<Rank> DominatingSpecies
        {
            get
            {
                return Simulation.PopulationManager.GetDominatingSpecies().Take(3);
            }
        }

        public SimulationDataViewModel(NavigationService navigationService, Simulation simulation) : base(navigationService)
        {
            Simulation = simulation;
            GoToHomeCommand = new RelayCommand(() => SelectHomeAsCurrentPage());
        }

        private void SelectHomeAsCurrentPage()
        {
            _navigationService.CurrentViewModel = new HomeViewModel(_navigationService);
        }

        private IDictionary<K, V> GetDataPoints<K, V>(IDictionary<K, V> dictionary)
        {
            int gap = (int)Math.Ceiling((double)dictionary.Count / MAX_AMOUNT_OF_DATAPOINTS);
            return dictionary.Where((data, index) => index % gap == 0).ToDictionary(data => data.Key, data => data.Value);
        }
    }
}
