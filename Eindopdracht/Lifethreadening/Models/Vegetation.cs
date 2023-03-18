using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Vegetation : SimulationElement
    {
        private object _nutritionLocker = new object();

        private const int DEFAULT_PRIORITY = 2;
        private const int MAX_NUTRITION_RELEASED_PER_DEPLETION = 10;

        private int _standardGrowth;
        private int _maxNutrition;
        private int _currentNutrition = 20;

        public Vegetation(string image, int standardGrowth, int maxNutrition, WorldContextService service): base(DEFAULT_PRIORITY, service)
        {
            _standardGrowth = standardGrowth;
            _maxNutrition = maxNutrition;
        }

        private void Grow(int growth)
        {
            lock(_nutritionLocker)
            {
                if(_currentNutrition + growth <= _maxNutrition)
                {
                    _currentNutrition += growth;
                }
            }
        }

        private void Grow(Weather weather)
        {
            Grow((int)(_standardGrowth * weather.RainFall + weather.Humidity));
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
