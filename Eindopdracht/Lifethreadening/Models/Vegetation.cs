using ColorCode.Compilation.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    /// <summary>
    /// This class is used to store data about a vegitation element
    /// </summary>
    public class Vegetation : SimulationElement
    {
        // Locking object for updating nutrition to prevent race conditions (for operations += and -= for example)
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

        /// <summary>
        /// Creates a new vegitation element
        /// </summary>
        /// <param name="image">The image used as icon for the vegitation</param>
        /// <param name="standardGrowth">The normal growth rate of the vegitation</param>
        /// <param name="maxNutrition">The max nutrition value this vegitation can have</param>
        /// <param name="service">The context service</param>
        public Vegetation(string image, int standardGrowth, int maxNutrition, WorldContextService service): base(DEFAULT_PRIORITY, image, service)
        {
            StandardGrowth = standardGrowth;
            MaxNutrition = maxNutrition;
        }

        [JsonConstructor]
        public Vegetation(string image, int standardGrowth, int maxNutrition) : this(image, standardGrowth, maxNutrition, null)
        {
        }

        /// <summary>
        /// This function grows a vegitation element by a given ammount
        /// </summary>
        /// <param name="growth">The amount the plants need to grow in nutrition</param>
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

        /// <summary>
        /// This function grows a plant based on the weather conditions
        /// </summary>
        /// <param name="weather">The current weather</param>
        private void Grow(Weather weather)
        {
            Grow((int)(StandardGrowth * weather.RainFall + weather.Temperature));
        }

        /// <summary>
        /// This function chekcs if a vegitation element still exisits in the world
        /// </summary>
        /// <returns></returns>
        public override bool StillExistsPhysically()
        {
            return _currentNutrition > 0;
        }

        /// <summary>
        /// This function gets the next action for the vegitation element
        /// </summary>
        /// <returns>The action this vegitation element wants to take next</returns>
        protected override Action GetNextAction()
        {
            Weather weather = ContextService.GetContext().Weather;
            return () => Grow(weather);
        }

        /// <summary>
        /// This function gets the nutritional value of the vegitation element
        /// </summary>
        /// <returns>The nutritional value of the vegitation</returns>
        public override int GetNutritionalValue()
        {
            return Math.Min(_currentNutrition, MAX_NUTRITION_RELEASED_PER_DEPLETION);
        }

        /// <summary>
        /// This function depletes nutritional value form a vegitation element
        /// </summary>
        /// <returns>The new nutritional value of the vegitation</returns>
        public override int DepleteNutritionalValue()
        {
            lock(_nutritionLocker)
            {
                int nutrition = GetNutritionalValue();
                _currentNutrition -= nutrition;
            }
            return _currentNutrition;
        }

        /// <summary>
        /// This function adds nutritional value to the vegitation
        /// </summary>
        /// <param name="nutrition">The ammount of nutrition to add</param>
        public void AddNutrition(int nutrition)
        {
            lock(_nutritionLocker)
            {
                _currentNutrition += nutrition;
            }
        }
    }
}
