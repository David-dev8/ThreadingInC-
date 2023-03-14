using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class StatisticInfo
    {
        public Color color { get; set; }
        public int value { get; set; }

        public StatisticInfo(Color color, int value)
        {
            this.color = color;
            this.value = value;
        }
    }
}
