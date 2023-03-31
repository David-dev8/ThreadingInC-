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
    /// <summary>
    /// This viewmodel is used for the simulation data view
    /// </summary>
    public class SimulationDataViewModel : BaseViewModel
    {
        public ICommand GoToHomeCommand { get; set; }

        public Simulation Simulation { get; set; }

        public IDictionary<StatisticInfo, int> AffectedStatistics
        {
            get
            {
                return Simulation.MutationManager.Analyze();
            }
        }

        public IDictionary<Species, IDictionary<DateTime, int>> SpeciesCount
        {
            get
            {
                return Simulation.PopulationManager.GetSpeciesCountPerSpecies();
            }
        }

        public IDictionary<DateTime, double> ShannonWeaverIndices
        {
            get
            {
                return Simulation.PopulationManager.GetShannonWeaverData();
            }
        }

        public IEnumerable<Rank> DominatingSpecies
        {
            get
            {
                return Simulation.PopulationManager.GetDominatingSpecies().Take(3);
            }
        }

        /// <summary>
        /// This function is used to navigate to the home view
        /// </summary>
        private void SelectHomeAsCurrentPage()
        {
            _navigationService.CurrentViewModel = new HomeViewModel(_navigationService);
        }

        /// <summary>
        /// Creates a new simulation data view model
        /// </summary>
        /// <param name="navigationService">The navigation service to use when navigating</param>
        /// <param name="simulation">The simulation that will get its data displayed</param>
        public SimulationDataViewModel(NavigationService navigationService, Simulation simulation) : base(navigationService)
        {
            Simulation = simulation;
            GoToHomeCommand = new RelayCommand(() => SelectHomeAsCurrentPage());
        }
    }
}
