﻿using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Enumeration;
using Windows.Devices.PointOfService;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
            DependencyProperty.Register("World", typeof(GridWorld), typeof(WorldView), new PropertyMetadata(null, new PropertyChangedCallback(InitializeRedraw)));

        public static readonly DependencyProperty SelectedSimulationElementProperty =
            DependencyProperty.Register("SelectedSimulationElement", typeof(SimulationElement), typeof(WorldView), new PropertyMetadata(null));

        private IDictionary<string, BitmapImage> elementImages = new Dictionary<string, BitmapImage>();

        public GridWorld World
        {
            get 
            { 
                return (GridWorld)GetValue(WorldProperty); 
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

        private Location[][] Locations
        {
            get
            {
                return World?.Grid;
            }
        }

        private double CellWidth
        {
            get
            {
                return Locations != null ? ActualWidth / Locations.Length : 0;
            }
        }

        private double CellHeight
        {
            get
            {
                return Locations != null ? ActualHeight / Locations[0].Length : 0;
            }
        }

        public WorldView()
        {
            this.InitializeComponent();
            this.SizeChanged += WorldView_SizeChanged;
        }

        private void WorldView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Draw();
        }

        private static void InitializeRedraw(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WorldView worldView = (WorldView)d;
            worldView.RedrawWhenChanged();
        }

        private void RedrawWhenChanged()
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

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            base.OnPointerPressed(e);
            SelectedSimulationElement = GetSimulationElementAt(e.GetCurrentPoint(this).Position);
        }

        private SimulationElement GetSimulationElementAt(Point point)
        {
            int column = (int)(point.X / CellWidth);
            int row = (int)(point.Y / CellHeight);
            return GetOnTop(Locations[row][column]);
        }

        private void Draw()
        {
            if(Locations != null)
            {
                Clear();
                for(int i = 0; i < Locations.Length; i++)
                {
                    Location[] row = Locations[i];
                    for(int j = 0; j < row.Length; j++)
                    {
                        Location location = row[j];
                        ShowAt(location, i, j);
                    }
                }
            }
        }

        private void ShowAt(Location location, int row, int column)
        {
            SimulationElement simulationElement = GetOnTop(location);
            if(simulationElement != null)
            {
                FrameworkElement representation = GetRepresentation(simulationElement);
                representation.Margin = new Thickness()
                {
                    Top = CellHeight * row,
                    Left = CellWidth * column,
                };
                Space.Children.Add(representation);
            }
        }

        private FrameworkElement GetRepresentation(SimulationElement simulationElement)
        {
            return new Image()
            {
                Source = GetImage(simulationElement.Image),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = CellHeight,
                Width = CellWidth
            };
        }

        private BitmapImage GetImage(string imageName)
        {
            if(!elementImages.ContainsKey(imageName))
            {
                var newImage = new BitmapImage(new Uri(new Uri("ms-appdata:///local/UserUploads/"), imageName));
                elementImages.Add(imageName, newImage);
            }
            return elementImages[imageName];
        }

        private SimulationElement GetOnTop(Location location)
        {
            return location.SimulationElements.FirstOrDefault();
        }

        private void Clear()
        {
            Space.Children.Clear();
        }
    }
}
