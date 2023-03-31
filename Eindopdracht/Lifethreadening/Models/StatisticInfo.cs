using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Lifethreadening.Models
{
    /// <summary>
    /// This class is used to store statistic information data
    /// </summary>
    public class StatisticInfo
    {
        public string Name { get; set; }
        public Color Color { get; set; }
        public int Value { get; set; }

        /// <summary>
        /// Creates a new statistics info object
        /// </summary>
        /// <param name="name">The name of the statistic this info is for</param>
        /// <param name="color">The color of the statistic</param>
        /// <param name="value">The value of the statistic</param>
        public StatisticInfo(string name, Color color, int value)
        {
            Name = name;
            Color = color;
            Value = value;
        }
    }
}
