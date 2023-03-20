using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Statistics
    {
        // TODO werkt dit?
        [Range(0, 100)]
        public int Weight { get; set; }
        public int Size { get; set; }
        public int Speed { get; set; }
        public int Aggresion { get; set; }
        public int Detection { get; set; }
        public int Resilience { get; set; }
        public int Intelligence { get; set; }
        public int SelfDefence { get; set; }
        public int MetabolicRate { get; set; }

        public Statistics Clone()
        {
            return new Statistics()
            {
                Speed = Speed,
                Weight = Weight,
                Size = Size,
                Aggresion = Aggresion,
                Detection = Detection,
                Resilience = Resilience,
                Intelligence = Intelligence,
                SelfDefence = SelfDefence,
                MetabolicRate = MetabolicRate
            };
        }

        public IDictionary<string, StatisticInfo> GetData()
        {
            // TODO
            return new Dictionary<string, StatisticInfo>
            {
                {"speed", new StatisticInfo("speed", ColorHelper.ToColor("#ffffff"), Speed) },
                {"weight", new StatisticInfo("weight", ColorHelper.ToColor("#ffffff"), Weight) },
                {"size", new StatisticInfo("size", ColorHelper.ToColor("#ffffff"), Size) },
                {"aggresion", new StatisticInfo("aggresion", ColorHelper.ToColor("#ffffff"), Aggresion) },
                {"detection", new StatisticInfo("detection", ColorHelper.ToColor("#ffffff"), Detection) },
                {"resilience", new StatisticInfo("resilience", ColorHelper.ToColor("#ffffff"), Resilience) },
                {"intelligence", new StatisticInfo("intelligence", ColorHelper.ToColor("#ffffff"), Intelligence) },
                {"selfDefence", new StatisticInfo("selfDefence", ColorHelper.ToColor("#ffffff"), SelfDefence) },
                {"metabolicRate", new StatisticInfo("metabolicRate", ColorHelper.ToColor("#ffffff"), MetabolicRate) }
            };
        }
    }
}
