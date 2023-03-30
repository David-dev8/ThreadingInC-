using Lifethreadening.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    internal class EmptyWorld : World
    {
        public EmptyWorld(Ecosystem ecosystem, DateTime currentDate, IWeatherManager weatherManager) : base(ecosystem, weatherManager)
        {
            CurrentDate = currentDate;
        }

        public override void CreateWorld()
        {
        }

        public override IEnumerable<Location> GetLocations()
        {
            return new List<Location>();
        }
    }
}
