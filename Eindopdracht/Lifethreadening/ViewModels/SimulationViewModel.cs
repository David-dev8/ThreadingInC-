using Lifethreadening.Base;
using Lifethreadening.DataAccess;
using Lifethreadening.DataAccess.Algorithmic;
using Lifethreadening.DataAccess.API;
using Lifethreadening.DataAccess.Database;
using Lifethreadening.DataAccess.JSON;
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
        private ISimulationReader _simulationReader;
        private ISimulationWriter _simulationWriter;
        private IWorldStateReader _worldStateReader;
        private IWorldStateWriter _worldStateWriter;

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
        private bool _saving;

        // TODO show spinner
        public bool Saving
        {
            get 
            { 
                return _saving; 
            }
            set 
            { 
                _saving = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand QuitCommand { get; set; }
        public ICommand ResumeCommand { get; set; }
        public ICommand PauseCommand { get; set; }
        public ICommand OpenInspectorCommand { get; set; }
        public ICommand SaveCommand { get; set; }
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
                return Simulation.World as GridWorld;
            }
        }

        public SimulationViewModel(NavigationService navigationService, Simulation simulation) : base(navigationService)
        {
            HasLoaded = false;
            Saving = false;
            _simulationReader = new DatabaseSimulationReader();
            _simulationWriter = new DatabaseSimulationWriter();
            _worldStateReader = new JSONWorldStateReader();
            _worldStateWriter = new JSONWorldStateWriter();
            QuitCommand = new RelayCommand(Quit);
            ResumeCommand = new RelayCommand(Start);
            PauseCommand = new RelayCommand(Stop);
            SaveCommand = new AsyncRelayCommand(Save);
            OpenInspectorCommand = new RelayCommand(OpenGeneInspector);
            Initialize(simulation);
        }

        private async Task Initialize(Simulation simulation)
        {
            try
            {
                if(simulation.Id == 0)
                {
                    await RegisterNewSimulation(simulation);
                }
                else
                {
                    await GetFullSimulation(simulation);
                }
                Simulation.PropertyChanged += Simulation_PropertyChanged;
                Simulation.Start();
                NotifyPropertyChanged(nameof(GridWorld));
                HasLoaded = true;
            }
            catch(Exception ex)
            {
                _navigationService.Error = new ErrorMessage("Something went wrong while initializing the simulation");
                Quit();
            }
        }

        private async Task RegisterNewSimulation(Simulation simulation)
        {
            Simulation = simulation;
            await Simulation.Setup();
            await Simulation.Save();
        }

        private async Task GetFullSimulation(Simulation simulation)
        {
            Simulation = await _simulationReader.ReadFullDetails(simulation);
            Simulation.World = await _worldStateReader.Read(Simulation.Filename);
            await Simulation.Setup(false);
            // TODO try catch stop simulation
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

        private void Stop()
        {
            Simulation.Stop();
        }

        private void Start()
        {
            Simulation.Start();
        }

        private async Task Save()
        {
            try
            {
                await InitiateSave();
            }
            catch(Exception ex)
            {
                _navigationService.Error = new ErrorMessage("Something went wrong while trying to save the data");
                Stop();
            }
        }

        private async Task InitiateSave()
        {
            Saving = true;
            await Simulation.Save();
            Saving = false;
        }

        private async Task NavigateToSimulationData()
        {
            try
            {
                string fileName = Simulation.Filename;
                Simulation.Filename = "";
                await InitiateSave();
                // Only if the save was succesful, we delete the save file
                await _worldStateWriter.Delete(fileName);
                _navigationService.CurrentViewModel = new SimulationDataViewModel(_navigationService, Simulation);
            }
            catch(Exception ex)
            {
                _navigationService.Error = new ErrorMessage("Something went wrong while wrapping up the simulation");
                Quit();
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            Simulation.End();
        }

        private void OpenGeneInspector() 
        {
            PopupVisible = true;
        }
    }
}
