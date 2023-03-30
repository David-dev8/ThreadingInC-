using Lifethreadening.Base;
using Lifethreadening.DataAccess.API;
using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lifethreadening.ViewModels
{
    public class SimulationViewModel : BaseViewModel
    {
        private Animal _selectedAnimal;

        private bool _popUpVisible = false;

        public bool PopupVisible
        {
            get 
            {
                return _popUpVisible;
            }
            set 
            {
                _popUpVisible = value;
                NotifyPropertyChanged();
            }
        }


        private bool _hasLoaded = false;

        public bool HasLoaded
        {
            get
            {
                return _hasLoaded;
            }
            set
            {
                _hasLoaded = value;
                NotifyPropertyChanged();
            }
        }
        public ICommand QuitCommand { get; set; }
        public ICommand ResumeCommand { get; set; }
        public ICommand PauseCommand { get; set; }
        public ICommand OpenInspector { get; set; }
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
            HasLoaded = false;

            QuitCommand = new RelayCommand(Quit);
            ResumeCommand = new RelayCommand(Simulation.Start);
            PauseCommand = new RelayCommand(Simulation.Stop);
            OpenInspector = new RelayCommand(OpenGeneInspector);

            Initialize();
        }

        private async void Initialize()
        {
            // TODO try catch stop simulation
            try
            {
                await Simulation.Setup();
                await Simulation.Save();
                Simulation.Start();
                HasLoaded = true;
            }
            catch(Exception ex)
            {
                _navigationService.Error = new ErrorMessage("Something went wrong while initializing the simulation");
                Quit();
            }
        }

        private void Simulation_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(Simulation.IsGameOver))
            {
                NavigateToSimulationData();
            }
        }

        private void Quit()
        {
            _navigationService.CurrentViewModel = new HomeViewModel(_navigationService);
        }

        private void NavigateToSimulationData()
        {
            _navigationService.CurrentViewModel = new SimulationDataViewModel(_navigationService, Simulation);
        }

        public override void Dispose()
        {
            base.Dispose();
            Simulation.End();
        }

        private void OpenGeneInspector() 
        {
            Mutation testter = new Mutation(MutationType.ADDITION, "test", "test", "terst", DateTime.Now, (s) => s.Weight = s.Weight - 5);
            testter.Affect(SelectedAnimal);
            PopupVisible = true;
        }
    }
}
