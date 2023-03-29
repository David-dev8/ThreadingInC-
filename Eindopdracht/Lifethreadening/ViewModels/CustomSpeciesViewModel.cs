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

namespace Lifethreadening.ViewModels
{
    public class CustomSpeciesViewModel : BaseViewModel
    {
        public readonly int MAX_POINTS = 350;

        public ICommand OpenImagePicker { get; set; }
        public ICommand SaveSpiecies { get; set; }

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

                //TODO Save custom spiecies to DB
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

                try { 
                    await saveFolder.GetFolderAsync("UserUploads");
                } catch( Exception e) {
                    await saveFolder.CreateFolderAsync("UserUploads");
                    await saveFolder.GetFolderAsync("UserUploads");
                }
                await file.CopyAsync(saveFolder);

                creatingSpecies.Image = saveFolder.Path + "/" + newFileName;
            }
        }

    }
}
