using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Enumeration;
using Windows.Devices.PointOfService;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Lifethreadening.Views.CustomControls
{
    public sealed partial class WorldView : UserControl
    {
        public static readonly DependencyProperty WorldProperty =
            DependencyProperty.Register("World", typeof(World), typeof(WorldView), new PropertyMetadata(null, new PropertyChangedCallback(InitializeRedraw)));

        public static readonly DependencyProperty SelectedSimulationElementProperty =
            DependencyProperty.Register("SelectedSimulationElement", typeof(SimulationElement), typeof(WorldView), new PropertyMetadata(null));

        public static BitmapImage img = new BitmapImage(new Uri("ms-appx:///Assets/fox.png")); // TODO dynamic

        public World World
        {
            get 
            { 
                return (World)GetValue(WorldProperty); 
            }
            set 
            { 
                SetValue(WorldProperty, value); 
            }
        }

        public SimulationElement SelectedSimulationElement
        {
            get
            {
                return (SimulationElement)GetValue(SelectedSimulationElementProperty);
            }
            set
            {
                SetValue(SelectedSimulationElementProperty, value);
            }
        }

        public Location[][] Locations
        {
            get
            {
                return ((GridWorld)World).Grid;
            }
        }

        public double CellWidth
        {
            get
            {
                return ActualWidth / Locations.Length;
            }
        }

        public double CellHeight
        {
            get
            {
                return ActualHeight / Locations[0].Length;
            }
        }

        public WorldView()
        {
            this.InitializeComponent();
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            base.OnPointerPressed(e);
            Point point = e.GetCurrentPoint(this).Position;

            double cellWidth = CellWidth;
            double cellHeight = CellHeight;
            
            int column = (int)(point.X / cellWidth);
            int row = (int)(point.Y / cellHeight);
            Location location = Locations[row][column];
            SimulationElement simulationElement = GetOnTop(location);
            SelectedSimulationElement = simulationElement;
        }

        private static void InitializeRedraw(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WorldView worldView = (WorldView)d;
            worldView.T();
            worldView.Draw();
        }

        private void T()
        {
            World.PropertyChanged += World_PropertyChanged;
        }

        private void World_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(World.Locations))
            {
                Draw();
            }
        }

        private void Draw()
        {
            double cellWidth = CellWidth;
            double cellHeight = CellHeight;

            Space.Children.Clear();

            Location[][] locations = Locations;
            for(int i = 0; i < locations.Length; i++)
            {
                Location[] row = locations[i];
                for(int j = 0; j < row.Length; j++)
                {
                    Location location = row[j];
                    SimulationElement simulationElement = GetOnTop(location);
                    if(simulationElement != null)
                    {
                        Image image = new Image()
                        {
                            Source = img,
                            Height = cellHeight,
                            Width = cellWidth,
                            Margin = new Thickness()
                            {
                                Top = cellHeight * i,
                                Left = cellWidth * j,
                                Bottom = 0,
                                Right = 0
                            },
                            VerticalAlignment = VerticalAlignment.Top,
                            HorizontalAlignment = HorizontalAlignment.Left,
                        };
                        Space.Children.Add(image);
                    }
                }
            }
        }

        private SimulationElement GetOnTop(Location location)
        {
            return location.SimulationElements.FirstOrDefault();
        }
    }
}
