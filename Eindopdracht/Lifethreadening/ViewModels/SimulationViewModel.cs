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
        private Animal _selectedAnimal;

        public Simulation Simulation { get; set; }
        public Animal SelectedAnimal 
        { 
            get
            {
                return _selectedAnimal;
            }
            set
            {
                _selectedAnimal = value;
                NotifyPropertyChanged();
            }
        }

        public SimulationElement SelectedSimulationElement
        {
            get
            {
                return SelectedAnimal;
            }
            set
            {
                SelectedAnimal = value as Animal;
            }
        }
        public bool HasSelectedAnimal
        {
            get
            {
                return SelectedAnimal != null;
            }
        }
        public GridWorld GridWorld
        {
            get
            {
                return (GridWorld)Simulation.World;
            }
        }

        public SimulationViewModel(NavigationService navigationService, Simulation simulation) : base(navigationService)
        {
            Simulation = simulation;
            Simulation.Start();
        }
    }
}
