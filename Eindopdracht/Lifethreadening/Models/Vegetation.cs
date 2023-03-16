using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Vegetation : SimulationElement
    {
        private const int DEFAULT_PRIORITY = 2;
        private const int MAX_NUTRITION_RELEASED_PER_DEPLETION = 10;

        private int _standardGrowth;
        private int _maxNutrition;
        private int _currentNutrition = 20;

        public Vegetation(string image, int standardGrowth, int maxNutrition): base(DEFAULT_PRIORITY)
        {
            _standardGrowth = standardGrowth;
            _maxNutrition = maxNutrition;
        }

        private void Grow(int growth)
        {
            if(_currentNutrition + growth <= _maxNutrition)
            {
                _currentNutrition += growth;
            }
        }

        public override bool StillExistsPhysically()
        {
            return _currentNutrition > 0;
        }

        protected override Action GetNextAction(WorldContext context)
        {
            return () => Grow((int)(_standardGrowth * context.Weather.RainFall + context.Weather.Humidity));
        }

        public override int GetNutritionalValue()
        {
            return Math.Min(_currentNutrition, MAX_NUTRITION_RELEASED_PER_DEPLETION);
        }

        public override int DepleteNutritionalValue()
        {
            int nutrition = GetNutritionalValue();
            _currentNutrition -= nutrition;
            return _currentNutrition;
        }
    }
}
