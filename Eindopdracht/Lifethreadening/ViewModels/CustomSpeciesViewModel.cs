using Lifethreadening.Base;
using Lifethreadening.DataAccess.Database;
using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.UI;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.UI.Xaml;
using System.Numerics;
using Windows.UI.Xaml.Controls;
using System.Xml.Linq;

namespace Lifethreadening.ViewModels
{
    /// <summary>
    /// This is the viewmodel for the custom spiecies view
    /// </summary>
    public class CustomSpeciesViewModel : BaseViewModel
    {

        public readonly int MAX_POINTS = 350;

        public ICommand OpenImagePickerCommand { get; set; }
        public ICommand SaveSpeciesCommand { get; set; }
        public ICommand QuitCommand { get; set; }

        public IEnumerable<Diet> PossibleDiets 
        {
            get 
            {
                return Enum.GetValues(typeof(Diet)).Cast<Diet>();
            }
        }

        public IEnumerable<Ecosystem> PossibleEcosystems
        {
            get 
            {
                DatabaseEcosystemReader reader = new DatabaseEcosystemReader();
                IEnumerable<Ecosystem> returnVal = reader.ReadAll();

                return returnVal; 
            }
        }

        private Ecosystem _chosenEcosystem;

        public Ecosystem ChosenEcosystem
        {
            get 
            { 
                return _chosenEcosystem;
            }
            set 
            { 
                _chosenEcosystem = value;
                NotifyPropertyChanged();
            }
        }

        private int _pointsLeft;
        public int PointsLeft 
        {
            get 
            { 
                return _pointsLeft;
            } 
            set 
            { 
                _pointsLeft = value;
                NotifyPropertyChanged();
            } 
        }

        public Species CreatingSpecies { get; set; }

        /// <summary>
        /// Creates a new custom species view model
        /// </summary>
        /// <param name="navigationService">The navigation service to use while navigating</param>
        public CustomSpeciesViewModel(NavigationService navigationService) : base(navigationService)
        {
            CreatingSpecies = new Species(0, "", "", "", "", 0, 0, 0, 0, PossibleDiets.First(), new Statistics());
            CreatingSpecies.BaseStatistics.PropertyChanged += BaseStatistics_PropertyChanged; 

            PointsLeft = MAX_POINTS - CreatingSpecies.BaseStatistics.GetSumOfStats();
            
            OpenImagePickerCommand = new AsyncRelayCommand(OpenImagePicker);
            SaveSpeciesCommand = new RelayCommand(CreateSpecies);
            QuitCommand = new RelayCommand(Quit);
            ChosenEcosystem = PossibleEcosystems.First();
        }

        /// <summary>
        /// This funcion is used to navigate to the home view
        /// </summary>
        private void Quit()
        {
            _navigationService.CurrentViewModel = new HomeViewModel(_navigationService);
        }

        /// <summary>
        /// This function is used to update the available stat point at the top of the screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseStatistics_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PointsLeft = MAX_POINTS - CreatingSpecies.BaseStatistics.GetSumOfStats();
        }


        /// <summary>
        /// This function is used to create a species and save it to the database
        /// </summary>
        private void CreateSpecies()
        {
            List<string> errors = CreatingSpecies.CheckIfValid();

            if (PointsLeft < 0)
            {
                errors.Add("* The points may not be negative");
            }

            if (errors.Count == 0) 
            {
                CreatingSpecies.MinBreedSize = (int)Math.Ceiling(CreatingSpecies.BreedSize / 2d);
                CreatingSpecies.MaxBreedSize = (int)Math.Ceiling(CreatingSpecies.BreedSize * 1.5d);
                CreatingSpecies.MaxAge = (int)Math.Ceiling(CreatingSpecies.AverageAge * 1.25d);
                CreatingSpecies.Description = "This is a user created species";

                TrySave();
            }
            else
            {
                _navigationService.Error = new ErrorMessage("Please fix the folowing errors to save this spiecies:\n" + 
                    string.Join("\n", errors), "There seem to be some errors with the data");
            }
        }

        private void TrySave()
        {
            try
            {
                DatabaseSpeciesWriter databaseWriter = new DatabaseSpeciesWriter();
                databaseWriter.Create(CreatingSpecies, ChosenEcosystem.Id);
                _navigationService.CurrentViewModel = new HomeViewModel(_navigationService);
            }
            catch(Exception)
            {
                _navigationService.Error = new ErrorMessage("The species could not be saved. Please try again.", "Save failed");
            }
        }

        /// <summary>
        /// This function is used to open the windows file picker to pick an image
        /// </summary>
        /// <returns></returns>
        private async Task OpenImagePicker()
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                string newFileName = file.Name.Split(".")[0] + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "." + file.Name.Split(".")[1];

                await file.RenameAsync(newFileName);
                StorageFolder saveFolder = ApplicationData.Current.LocalFolder; 

                saveFolder = await saveFolder.CreateFolderAsync("UserUploads", CreationCollisionOption.OpenIfExists);

                await file.CopyAsync(saveFolder);

                CreatingSpecies.Image = newFileName;
            }
        }
    }
}
