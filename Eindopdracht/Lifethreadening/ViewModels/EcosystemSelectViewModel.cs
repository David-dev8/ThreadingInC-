using Lifethreadening.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.ViewModels
{
    public class EcosystemSelectViewModel : BaseViewModel
    {
        private ObservableCollection<string> _images;
        private string _selectedImage;
        public ObservableCollection<string> Images
        {
            get { return _images; }
            set
            {
                if (_images != value)
                {
                    _images = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedImage
        {
            get { return _selectedImage; }
            set
            {
                if (_selectedImage != value)
                {
                    _selectedImage = value;
                    OnPropertyChanged();
                }
            }
        }
        public EcosystemSelectViewModel(NavigationService navigationService) : base(navigationService)
        {
            // Initialize the images collection with some URLs
            Images = new ObservableCollection<string>()
        {
            "https://placeimg.com/380/230/animals",
            "https://placeimg.com/380/230/tech",
            "https://placeimg.com/380/230/nature"
        };

            // Set the selected image to the first image URL in the collection
            SelectedImage = Images[0];
        }

    }
}
