using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class WorldContext
    {
        public Ecosystem Ecosystem { get; set; }
        public Weather Weather { get; set; }
        public DateTime Date { get; set; }

        public WorldContext(Ecosystem ecosystem, Weather weather, DateTime date)
        {
            Ecosystem = ecosystem;
            Weather = weather;
            Date = date;
        }
    }
}
