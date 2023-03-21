using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lifethreadening.Base;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Statistics : Observable
    {

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

        public Statistics()
        {
            Weight = 0;
            Size = 0;
            Speed = 0;
            Aggresion = 0;
            Detection = 0;
            Resilience = 0;
            Intelligence = 0;
            SelfDefence = 0;
            MetabolicRate = 0;
        }

        public int GetSumOfStats()
        {
            return Weight + Size + Speed + Aggresion + Detection + Resilience + Intelligence + SelfDefence + MetabolicRate;
        }
    }
}
