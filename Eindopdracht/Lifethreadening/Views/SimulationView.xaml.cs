﻿using Lifethreadening.Models;
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

            SizeChanged += SimulationView_SizeChanged;
        }

        private void SimulationView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            gridMutation.Width = ActualWidth / 2;
            gridMutation.Height = ActualHeight / 2;
            geneticsView.HorizontalOffset = (ActualWidth - gridMutation.ActualWidth) / 4;
            geneticsView.VerticalOffset = (ActualHeight - gridMutation.ActualHeight) / 4;
        }

    }
}
