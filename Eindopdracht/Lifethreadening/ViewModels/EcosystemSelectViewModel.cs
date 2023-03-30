using Lifethreadening.Base;
using Lifethreadening.DataAccess;
using Lifethreadening.DataAccess.Algorithmic;
using Lifethreadening.DataAccess.Database;
using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Lifethreadening.ViewModels
{
    public class EcosystemSelectViewModel : BaseViewModel
    {
        private readonly IEcosystemReader _ecosystemReader;
        private Ecosystem _selectedEcosystem;

        public IEnumerable<Ecosystem> Ecosystems { get; set; }
        public Ecosystem SelectedEcosystem
        {
            get 
            { 
                return _selectedEcosystem; 
            }
            set
            {
                if (_selectedEcosystem != value)
                {
                    _selectedEcosystem = value;
                    OnPropertyChanged(nameof(SelectedEcosystem));
                }
            }
        }

        public ICommand SelectEcosystemCommand { get; set; }
        public ICommand GoBackCommand { get; set; }


        public EcosystemSelectViewModel(NavigationService navigationService) : base(navigationService)
        {
            _ecosystemReader = new DatabaseEcosystemReader();
            Ecosystems = _ecosystemReader.ReadAll();
            SelectedEcosystem = Ecosystems.First();

            SelectEcosystemCommand = new RelayCommand(SelectEcosystem);
            GoBackCommand = new RelayCommand(GoBack);
        }

        private void SelectEcosystem()
        {
            // Set the current view model to a new instance of SimulationViewModel with the selected ecosystem
            _navigationService.CurrentViewModel = new SimulationViewModel(_navigationService, new Simulation("Amazon", new GridWorld(SelectedEcosystem,new RandomWeatherManager())));
        }

        private void GoBack()
        {
            _navigationService.CurrentViewModel = new HomeViewModel(_navigationService);
        }
    }
}
