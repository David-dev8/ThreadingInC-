using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Lifethreadening.Base;
using System.Threading.Tasks;
using Lifethreadening.ExtensionMethods;

namespace Lifethreadening.Models
{
    public class Statistics : Observable
    {
        private static readonly IDictionary<string, Windows.UI.Color> _colors = new Dictionary<string, Windows.UI.Color>()
        {
            { "speed", ColorHelper.ToColor("#ECB72F") },
            { "weight", ColorHelper.ToColor("#6F733E") },
            { "size", ColorHelper.ToColor("#4D4E47") },
            { "aggresion", ColorHelper.ToColor("#BD4400") },
            { "detection", ColorHelper.ToColor("#97BD00") },
            { "resilience", ColorHelper.ToColor("#860681") },
            { "intelligence", ColorHelper.ToColor("#37A195") },
            { "selfDefence", ColorHelper.ToColor("#584DDB") },
            { "metabolicRate", ColorHelper.ToColor("#32DE24") },
        };

        private int _weight;
        public int Weight
        {
            get
            {
                return _weight;
            }
            set
            {
                _weight = value;
                NotifyPropertyChanged();
            }
        }

        private int _size;
        public int Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
                NotifyPropertyChanged();
            }
        }

        private int _speed;
        public int Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
                NotifyPropertyChanged();
            }
        }

        private int _agression;
        public int Aggresion
        {
            get
            {
                return _agression;
            }
            set
            {
                _agression = value;
                NotifyPropertyChanged();
            }
        }

        private int _detection;
        public int Detection
        {
            get
            {
                return _detection;
            }
            set
            {
                _detection = value;
                NotifyPropertyChanged();
            }
        }

        private int _resilience;
        public int Resilience
        {
            get
            {
                return _resilience;
            }
            set
            {
                _resilience = value;
                NotifyPropertyChanged();
            }
        }

        private int _intelligence;
        public int Intelligence
        {
            get
            {
                return _intelligence;
            }
            set
            {
                _intelligence = value;
                NotifyPropertyChanged();
            }
        }

        private int _selfDefence;
        public int SelfDefence
        {
            get
            {
                return _selfDefence;
            }
            set
            {
                _selfDefence = value;
                NotifyPropertyChanged();
            }
        }

        private int _metabolicRate;
        public int MetabolicRate
        {
            get
            {
                return _metabolicRate;
            }
            set
            {
                _metabolicRate = value;
                NotifyPropertyChanged();
            }
        }

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

        public int GetSumOfStats()
        {
            return Weight + Size + Speed + Aggresion + Detection + Resilience + Intelligence + SelfDefence + MetabolicRate;

        }

        public IDictionary<string, StatisticInfo> GetData()
        {
            IDictionary<string, StatisticInfo> stats = new Dictionary<string, StatisticInfo>();
            Add(stats, "speed", Speed);
            Add(stats, "weight", Weight);
            Add(stats, "size", Size);
            Add(stats, "aggresion", Aggresion);
            Add(stats, "detection", Detection);
            Add(stats, "resilience", Resilience);
            Add(stats, "intelligence", Intelligence);
            Add(stats, "selfDefence", SelfDefence);
            Add(stats, "metabolicRate", MetabolicRate);
            return stats;
        }

        private void Add(IDictionary<string, StatisticInfo> stats, string name, int value)
        {
            stats.Add(name, new StatisticInfo(name, _colors[name], value));
        }
    }
}
