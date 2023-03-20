using Lifethreadening.Base;
using Lifethreadening.DataAccess.Database;
using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lifethreadening.ViewModels
{
    public class CustomSpeciesViewModel : BaseViewModel
    {
        public readonly int MAX_POINTS = 350;

        public ICommand OpenImagePicker { get; set; }

        public int pointsLeft { get; set; }
        public Species creatingSpecies { get; set; }

        public CustomSpeciesViewModel(NavigationService navigationService) : base(navigationService)
        {
            pointsLeft = MAX_POINTS;
            creatingSpecies = new Species();

            OpenImagePicker = new
        }



    }
}
