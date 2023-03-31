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


namespace Lifethreadening.Views.CustomControls
{
    /// <summary>
    /// This is a custom view control that displays the game / simulation world
    /// </summary>
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
                return World.Grid;
            }
        }

        private double CellWidth
        {
            get
            {
                return ActualWidth / Locations.Length;
            }
        }

        private double CellHeight
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

        /// <summary>
        /// This method gets a simulation element at the given point
        /// </summary>
        /// <param name="point">The point to look for an element at</param>
        /// <returns>The element at the given point</returns>
        private SimulationElement GetSimulationElementAt(Point point)
        {
            int column = (int)(point.X / CellWidth);
            int row = (int)(point.Y / CellHeight);
            return GetOnTop(Locations[row][column]);
        }

        /// <summary>
        /// This methodd draws the world to the screen
        /// </summary>
        private void Draw()
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

        /// <summary>
        /// This method assigns a location to a point in the world
        /// </summary>
        /// <param name="location">The location to show</param>
        /// <param name="row">The row it should be in</param>
        /// <param name="column">The column it should be in</param>
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

        /// <summary>
        /// This method gets how a element should be represented
        /// </summary>
        /// <param name="simulationElement">The element that needs to represented</param>
        /// <returns>A framework element of that element</returns>
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

        /// <summary>
        /// This method gets a bitmap Image based on a filename
        /// </summary>
        /// <param name="imageName">The name of the image to get</param>
        /// <returns>The image that was requested as abitmap</returns>
        private BitmapImage GetImage(string imageName)
        {
            // TODO use converter
            if(!elementImages.ContainsKey(imageName))
            {
                var newImage = new BitmapImage(new Uri(new Uri("ms-appdata:///local/UserUploads/"), imageName));
                elementImages.Add(imageName, newImage);
            }
            return elementImages[imageName];
        }

        /// <summary>
        /// This method gets the first simulation element in the given location
        /// </summary>
        /// <param name="location">The location to get the top element from</param>
        /// <returns>The top most element in the given location</returns>
        private SimulationElement GetOnTop(Location location)
        {
            return location.SimulationElements.FirstOrDefault();
        }

        /// <summary>
        /// This method empties the world
        /// </summary>
        private void Clear()
        {
            Space.Children.Clear();
        }
    }
}
