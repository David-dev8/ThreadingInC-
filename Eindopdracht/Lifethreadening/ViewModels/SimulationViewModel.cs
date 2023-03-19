using Lifethreadening.Base;
using Lifethreadening.DataAccess.API;
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
            Simulation.PropertyChanged += Simulation_PropertyChanged;


            var s = new APIGeneReader();
            var b = s.GetRandomGene();
        }

        private void Simulation_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(Simulation.IsGameOver))
            {
                NavigateToSimulationData();
            }
        }

        private void NavigateToSimulationData()
        {
            _navigationService.CurrentViewModel = new SimulationDataViewModel(_navigationService);
        }

        public override void Dispose()
        {
            base.Dispose();
            Simulation.Dispose();
        }
    }
}
