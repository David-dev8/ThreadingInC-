using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Lifethreadening.Models
{
    public class StatisticInfo
    {
        public string Name { get; set; }
        public Color Color { get; set; }
        public int Value { get; set; }

        public StatisticInfo(string name, Color color, int value)
        {
            Name = name;
            Color = color;
            Value = value;
        }
    }
}
