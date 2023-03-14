using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Path
    {
        public Path PreviousPath { get; set; }
        public Location CurrentLocation { get; set; }
        public int Length
        {
            get
            {
                return 0;
            }
        }

        public Path(Location currentLocation, Path previousPath = null)
        {
            CurrentLocation = currentLocation;
            PreviousPath = previousPath;
        }
    }
}
