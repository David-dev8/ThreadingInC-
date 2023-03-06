using Lifethreadening.Base;
using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.ViewModels
{
    public class SimulationViewModel : BaseViewModel
    {
        public Simulation Simulation { get; set; }

        public SimulationViewModel(NavigationService navigationService, Simulation simulation) : base(navigationService)
        {
            Simulation = simulation;
        }
    }
}
