using Lifethreadening.Base;
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
        private static readonly int MAX_AMOUNT_OF_DATAPOINTS = 100;
        
        public ICommand GoToHomeCommand { get; set; }

        public Simulation Simulation { get; set; }

        public IDictionary<StatisticInfo, int> AffectedStatistics { get; set; }

        public IDictionary<Species, IDictionary<DateTime, int>> SpeciesCount { get; set; }

        public IDictionary<DateTime, double> ShannonWeaverIndices { get; set; }

        public IEnumerable<Rank> DominatingSpecies { get; set; }

        public SimulationDataViewModel(NavigationService navigationService, Simulation simulation) : base(navigationService)
        {
            Simulation = simulation;
            GoToHomeCommand = new RelayCommand(() => SelectHomeAsCurrentPage());

            InitializeData();
        }

        private async Task InitializeData()
        {
            try
            {
                IDictionary<Species, IDictionary<DateTime, int>> speciesCount = Simulation.PopulationManager.GetSpeciesCountPerSpecies();
                SpeciesCount = speciesCount.ToDictionary(currentSpeciesCount => currentSpeciesCount.Key, currentSpeciesCount => GetDataPoints(currentSpeciesCount.Value));
                ShannonWeaverIndices = GetDataPoints(Simulation.PopulationManager.GetShannonWeaverData());
                DominatingSpecies = Simulation.PopulationManager.GetDominatingSpecies().Take(3);
                AffectedStatistics = Simulation.MutationManager.Analyze();
            }
            catch(Exception)
            {
                _navigationService.Error = new ErrorMessage("Data could not be fetched");
                await Task.Delay(100); // TODO
                SelectHomeAsCurrentPage();
            }
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
