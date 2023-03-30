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
        private IEnumerable<Ecosystem> _images;
        private Ecosystem _selectedImage;

        public IEnumerable<Ecosystem> Images
        {
            get { return _images; }
            set
            {
                if (_images != value)
                {
                    _images = value;
                   // OnPropertyChanged();
                }
            }
        }
        public Ecosystem SelectedImage
        {
            get 
            { 
                return _selectedImage; 
            }
            set
            {
                if (_selectedImage != value)
                {
                    _selectedImage = value;
                    OnPropertyChanged(nameof(SelectedImage));
                }
            }
        }

        public ICommand SelectEcosystemCommand { get; set; }


        public EcosystemSelectViewModel(NavigationService navigationService) : base(navigationService)
        {
            _ecosystemReader = new DatabaseEcosystemReader();

            // Initialize the images collection
            Images = _ecosystemReader.ReadAll();


            // Set the selected image to the first image in the collection 
            SelectedImage = Images.First();

            SelectEcosystemCommand = new RelayCommand(SelectEcosystem);

        }

        private void SelectEcosystem()
        {
            // Set the current view model to a new instance of SimulationViewModel with the selected ecosystem
            //_navigationService.CurrentViewModel = new SimulationViewModel(_navigationService, new Simulation("Amazon", new GridWorld(SelectedImage,new RandomWeatherManager())));
        }

    }
}
