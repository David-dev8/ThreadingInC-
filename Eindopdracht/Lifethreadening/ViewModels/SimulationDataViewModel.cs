using Lifethreadening.Base;
using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.ViewModels
{
    public class SimulationDataViewModel : BaseViewModel
    {
        public Simulation Simulation { get; set; }

        public IDictionary<StatisticInfo, int> AffectedStatistics
        {
            get
            {
                return Simulation.MutationManager.Analyze();
            }
        }

        public IDictionary<DateTime, double> ShannonWeaverIndices
        {
            get
            {
                var t = Simulation.PopulationManager.GetShannonWeaverData();
                return t;
            }
        }

        public SimulationDataViewModel(NavigationService navigationService, Simulation simulation) : base(navigationService)
        {
            Simulation = simulation;
        }
        

    }
}
