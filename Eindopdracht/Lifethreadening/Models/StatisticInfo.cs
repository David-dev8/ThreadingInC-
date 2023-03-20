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
        public string Statistic { get; set; }
        public Color color { get; set; }
        public int value { get; set; }

        public StatisticInfo(string statistic, Color color, int value)
        {
            Statistic = statistic;
            this.color = color;
            this.value = value;
        }
    }
}
