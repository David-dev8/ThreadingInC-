using Lifethreadening.Base;
using Lifethreadening.DataAccess;
using Lifethreadening.DataAccess.Database;
using Lifethreadening.Models;
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
                    OnPropertyChanged();
                }
            }
        }
        public Ecosystem SelectedImage
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
            _ecosystemReader = new DatabaseEcosystemReader();

            // Initialize the images collection with some URLs
            Images = _ecosystemReader.ReadAll();
       

            // Set the selected image to the first image URL in the collection
            SelectedImage = Images.First();
        }
    }
}
