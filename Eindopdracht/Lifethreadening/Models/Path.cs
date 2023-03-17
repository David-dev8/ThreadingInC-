using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Reflection;
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
                Path path = this;
                int i = 0;
                while (path.PreviousPath != null)
                {
                    path = path.PreviousPath;
                    i++;
                }
                return i;
            }
        }

        public Path(Location currentLocation, Path previousPath = null)
        {
            CurrentLocation = currentLocation;
            PreviousPath = previousPath;
        }

        public Location GetLocationAt(int index)
        {
            if(index < 0 || index > Length)
            {
                return null;
            }

            Path path = this;
            for(int i = 0; i < Length - index; i++)
            {
                path = path.PreviousPath;
            }
            return path.CurrentLocation;
        }
    }
}
