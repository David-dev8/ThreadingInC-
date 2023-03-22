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

namespace Lifethreadening.ViewModels
{
    public class CustomSpeciesViewModel : BaseViewModel
    {
        public readonly int MAX_POINTS = 350;

        public ICommand OpenImagePicker { get; set; }

        private int _pointsLeft;
        public int PointsLeft {
            get { 
                return _pointsLeft;
            } set { 
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
        }

        private void BaseStatistics_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PointsLeft = MAX_POINTS - creatingSpecies.BaseStatistics.GetSumOfStats(); //TODO GetSumOfStats verwerken
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
