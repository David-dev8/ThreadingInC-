using ColorCode.Compilation.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Vegetation : SimulationElement
    {
        private object _nutritionLocker = new object();

        private const int DEFAULT_PRIORITY = 2;
        private const int MAX_NUTRITION_RELEASED_PER_DEPLETION = 10;

        public int StandardGrowth { get; set; }
        public int MaxNutrition { get; set; }

        private int _currentNutrition = 20;
        [JsonInclude]
        public int CurrentNutrition
        {
            get
            {
                return _currentNutrition;
            }
            private set
            {
                _currentNutrition = Math.Min(Math.Max(0, value), MaxNutrition);
            }
        }

        public Vegetation(string image, int standardGrowth, int maxNutrition, WorldContextService service): base(DEFAULT_PRIORITY, image, service)
        {
            StandardGrowth = standardGrowth;
            MaxNutrition = maxNutrition;
        }

        [JsonConstructor]
        public Vegetation(string image, int standardGrowth, int maxNutrition) : this(image, standardGrowth, maxNutrition, null)
        {
        }

        private void Grow(int growth)
        {
            lock(_nutritionLocker)
            {
                if(_currentNutrition + growth <= MaxNutrition)
                {
                    _currentNutrition += growth;
                }
            }
        }

        private void Grow(Weather weather)
        {
            Grow((int)(StandardGrowth * weather.RainFall + weather.Temperature));
        }

        public override bool StillExistsPhysically()
        {
            return _currentNutrition > 0;
        }

        protected override Action GetNextAction()
        {
            Weather weather = ContextService.GetContext().Weather;
            return () => Grow(weather);
        }

        public override int GetNutritionalValue()
        {
            return Math.Min(_currentNutrition, MAX_NUTRITION_RELEASED_PER_DEPLETION);
        }

        public override int DepleteNutritionalValue()
        {
            lock(_nutritionLocker)
            {
                int nutrition = GetNutritionalValue();
                _currentNutrition -= nutrition;
            }
            return _currentNutrition;
        }

        public void AddNutrition(int nutrition)
        {
            lock(_nutritionLocker)
            {
                _currentNutrition += nutrition;
            }
        }
    }
}
