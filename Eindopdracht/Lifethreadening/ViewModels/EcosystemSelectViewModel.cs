using Lifethreadening.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.ViewModels
{
    public class ImageInfo
    {
        public string Url { get; set; }
        public string Text { get; set; }
    }
    public class EcosystemSelectViewModel : BaseViewModel
    {
        private ObservableCollection<ImageInfo> _images;
        private ImageInfo _selectedImage;

        public ObservableCollection<ImageInfo> Images
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
        public ImageInfo SelectedImage
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
            Images = new ObservableCollection<ImageInfo>()
        {
                new ImageInfo() { Url = "https://placeimg.com/380/230/animals", Text = "Jurrasic era" },
                new ImageInfo() { Url = "https://placeimg.com/380/230/tech", Text = "Antartica" },
                new ImageInfo() { Url = "https://placeimg.com/380/230/nature", Text = "Skull island" },
        };

            // Set the selected image to the first image URL in the collection
            SelectedImage = Images[0];
        }
    }
}
