using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Lifethreadening.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SimulationView : Page
    {

        private ObservableCollection<Mutation> mutationsTester { get; set; }

        public SimulationView()
        {
            this.InitializeComponent();

            mutationsTester = new ObservableCollection<Mutation>();
            Dictionary<string, StatisticInfo> affections = new Dictionary<string, StatisticInfo>();
            affections.Add("Speed", new StatisticInfo(Color.Red, +12));
            affections.Add("Size", new StatisticInfo(Color.Red, -5));
            mutationsTester.Add(new Mutation(MutationType.ADDITION, "7384.43", "glycin", "bancin", DateTime.Now, affections));
            mutationsTester.Add(new Mutation(MutationType.ADDITION, "3421.33", "glycin", "bancin", DateTime.Now, affections));
            mutationsTester.Add(new Mutation(MutationType.ADDITION, "8356.73", "glycin", "bancin", DateTime.Now, affections));
        }

        private void GeneInspectorOpen_Click(object sender, RoutedEventArgs e)
        {
            geneticsView.IsOpen = true;
        }
    }
}
