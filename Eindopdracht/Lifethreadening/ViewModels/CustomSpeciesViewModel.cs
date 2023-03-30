using Lifethreadening.Base;
using Lifethreadening.DataAccess.Database;
using Lifethreadening.Models;
using SQLitePCL;
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
using WinRTXamlToolkit.Tools;

namespace Lifethreadening.ViewModels
{
    public class CustomSpeciesViewModel : BaseViewModel
    {

        public readonly int MAX_POINTS = 350;

        public ICommand OpenImagePicker { get; set; }
        public ICommand SaveSpiecies { get; set; }

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



        private List<string> _errors;
        public List<string> Errors
        {
            get 
            {
                return _errors;
            }
            set 
            {
                _errors = value; 
                NotifyPropertyChanged();
            }
        }


        private bool _hasErrors = false;
        public bool HasErrors 
        {
            get 
            {
                return _hasErrors;
            }
            set 
            {
                _hasErrors = value; 
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

        public Species creatingSpecies { get; set; }

        public CustomSpeciesViewModel(NavigationService navigationService) : base(navigationService)
        {
            creatingSpecies = new Species();
            creatingSpecies.BaseStatistics.PropertyChanged += BaseStatistics_PropertyChanged; 

            PointsLeft = MAX_POINTS - creatingSpecies.BaseStatistics.GetSumOfStats();
            
            OpenImagePicker = new AsyncRelayCommand(OpenFilePicker);
            SaveSpiecies = new RelayCommand(CreateSpiecies);

            ChosenEcosystem = PossibleEcosystems.First();
        }

        private void BaseStatistics_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PointsLeft = MAX_POINTS - creatingSpecies.BaseStatistics.GetSumOfStats(); //TODO GetSumOfStats verwerken
        }

        private void CreateSpiecies()
        {

            List<string> valid = creatingSpecies.CheckIfValid();

            if (PointsLeft < 0)
            {
                valid.Add("* The points may not be negative");
            }

            HasErrors = valid.Count > 0;
            Errors = valid;

            if (valid.Count == 0) 
            {
                creatingSpecies.MinBreedSize = (int)Math.Ceiling(creatingSpecies.BreedSize / 2d);
                creatingSpecies.MaxBreedSize = (int)Math.Ceiling(creatingSpecies.BreedSize * 1.5d);
                creatingSpecies.MaxAge = (int)Math.Ceiling(creatingSpecies.AverageAge * 1.25d);
                creatingSpecies.Description = "This is an user created species";

                DatabaseSpeciesWriter DatabaseWriter = new DatabaseSpeciesWriter();
                DatabaseWriter.Create(creatingSpecies, ChosenEcosystem.Id);
                _navigationService.CurrentViewModel = new HomeViewModel(_navigationService);
            }

        }

        private async Task OpenFilePicker()
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

                creatingSpecies.Image = newFileName;
            }
        }

    }
}
