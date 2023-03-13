using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Vegetation : SimulationElement
    {
        private int _standardGrowth;
        private int _maxNutrition;
        private int _currentNutrition;

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
    }
}
