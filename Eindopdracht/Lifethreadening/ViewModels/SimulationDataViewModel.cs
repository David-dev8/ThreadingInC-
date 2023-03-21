﻿using Lifethreadening.Base;
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

        public SimulationDataViewModel(NavigationService navigationService, Simulation simulation) : base(navigationService)
        {
            Simulation = simulation;
        }
        

    }
}
