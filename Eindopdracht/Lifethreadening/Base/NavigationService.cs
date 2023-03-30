using Lifethreadening.ViewModels;
using Lifethreadening.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Lifethreadening.Base
{
    /// <summary>
    /// The navigation service is used to navigate between pages
    /// </summary>
    public class NavigationService: Observable
    {
        private const string CLOSE_DIALOG_TEXT = "Continue";

        private Frame _frame;
        private BaseViewModel _currentViewModel;
        private readonly Dictionary<Type, Type> viewMapping = new Dictionary<Type, Type>() 
        {
            { typeof(CustomSpeciesViewModel), typeof(CustomSpeciesView) },
            { typeof(EcosystemSelectViewModel), typeof(EcosystemSelectView) },
            { typeof(HomeViewModel), typeof(HomeView) },
            { typeof(SimulationDataViewModel), typeof(SimulationDataView) },
            { typeof(SimulationViewModel), typeof(SimulationView) }
        };

        public BaseViewModel CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                _currentViewModel?.Dispose();
                _currentViewModel = value;
                _frame.Navigate(viewMapping[_currentViewModel.GetType()]);
                _frame.DataContext= _currentViewModel;
            }
        }

        private ContentDialog _dialog;
        private ErrorMessage _error;
        public ErrorMessage Error
        {
            get
            {
                return _error;
            }
            set
            {
                _error = value;
                if(_error != null)
                {
                    _dialog = new ContentDialog()
                    {
                        Title = _error.Title,
                        Content = _error.Content,
                        CloseButtonText = CLOSE_DIALOG_TEXT
                    };
                    _dialog.ShowAsync();
                }
                else
                {
                    _dialog.Hide();
                }
            }
        }

        /// <summary>
        /// Create a new navigationservice
        /// </summary>
        /// <param name="frame">The frame to navigate on</param>
        public NavigationService(Frame frame)
        {
            _frame = frame;
        }
    }

    /// <summary>
    /// This class is used to contain an error message
    /// </summary>
    public class ErrorMessage
    {
        private const string DEFAULT_TITLE = "Error";

        public string Title { get; set; }
        public string Content { get; set; }

        /// <summary>
        /// Creates a new eroor message
        /// </summary>
        /// <param name="content">The main body of the error</param>
        /// <param name="title">The title of this error, leaving this blank wil set it to "Error"</param>
        public ErrorMessage(string content, string title = DEFAULT_TITLE)
        {
            Title = title;
            Content = content;
        }
    }
}
