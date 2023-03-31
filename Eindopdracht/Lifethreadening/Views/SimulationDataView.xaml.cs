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
    /// This view is used to display data about completed games in graphs and charts
    /// </summary>
    public sealed partial class SimulationDataView : Page
    {
        public SimulationDataView()
        {
            this.InitializeComponent();
        }
    }
}
