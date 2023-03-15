using Lifethreadening.Models;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Lifethreadening.Views.CustomControls
{
    public sealed partial class WorldView : UserControl
    {
        public static readonly DependencyProperty WorldProperty =
            DependencyProperty.Register("World", typeof(World), typeof(WorldView), new PropertyMetadata(null));

        public World World
        {
            get { 
                return (World)GetValue(WorldProperty); }
            set { 
                SetValue(WorldProperty, value); }
        }

        public GridWorld GridWorld
        {
            get
            {
                return ((GridWorld)World);
            }
        }

        public double CellWidth
        {
            get
            {
                return ActualWidth / GridWorld.Width;
            }
        }

        public double CellHeight
        {
            get
            {
                return ActualHeight / GridWorld.Height;
            }
        }

        public WorldView()
        {
            this.InitializeComponent();
        }
    }
}
