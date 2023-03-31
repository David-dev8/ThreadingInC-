using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Lifethreadening.Views
{
    /// <summary>
    /// This view is used to create a custom species
    /// </summary>
    public sealed partial class CustomSpeciesView : Page
    {
        public CustomSpeciesView()
        {
            this.InitializeComponent();
            SizeChanged += CustomSpeciesView_LayoutUpdated1; ;
        }

        /// <summary>
        /// Positions the popup on the frame resizing ensuring that its always centered in the frame
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomSpeciesView_LayoutUpdated1(object sender, object e)
        {
            ValidationPopup.HorizontalOffset = (ActualWidth - ValidationPopup.ActualWidth/2) / 2;
            ValidationPopup.VerticalOffset = (ActualHeight - ValidationPopup.ActualHeight/2) / 2;
        }

    }
}
