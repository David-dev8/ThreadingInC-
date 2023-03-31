using Lifethreadening.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Disasters
{
    /// <summary>
    /// This class is used to contain data about lightning disasters
    /// </summary>
    public class LightningDisaster : Disaster
    {
        private const string DESCRIPTION = "Thunderstorm";
        private const int MIN_DAMAGE = 1;
        private const int MAX_DAMAGE = 5;
        private const int MIN_SPARKS = 1;
        private const int MAX_SPARKS = 5000;
        private const double ACTIVITY_LEVEL_LOG_BASE = 6;

        private Random _random = new Random();
        private int _sparks;

        /// <summary>
        /// Creates a new lightning disaster
        /// </summary>
        /// <param name="contextService">The data context</param>
        public LightningDisaster(WorldContextService contextService) : base(DESCRIPTION, contextService)
        {
            _sparks = _random.Next(MIN_SPARKS, MAX_SPARKS);
        }

        public override void Strike(IEnumerable<SimulationElement> simulationElements)
        {
            // Fire sparks and hits
            IEnumerable<int> sparks = Enumerable.Range(0, _sparks)
                        .Select(r => _random.Next(MIN_DAMAGE, MAX_DAMAGE))
                        .ToList();
            foreach(int spark in sparks)
            {
                Damage(simulationElements.GetRandom(), spark);
            }
        }

        /// <summary>
        /// This method damages lightningstruck animals and vegitation
        /// </summary>
        /// <param name="simulationElement">The element to damage</param>
        /// <param name="damage">The amount of damage</param>
        private void Damage(SimulationElement simulationElement, int damage)
        {
            if(simulationElement is Animal animal)
            {
                animal.AddHp(-damage);
            }
            else if(simulationElement is Vegetation vegetation)
            {
                vegetation.AddNutrition(-damage);
            }
        }

        public override string GetLongDescription()
        {
            return $"LightningDisaster. Initated at: {DateInitiated:d}. Lightning activity level: {CalculateActivityLevel():0.00}.";
        }

        /// <summary>
        /// this method calculates how active the storm is
        /// </summary>
        /// <returns>A double indicating how active the storm is</returns>
        private double CalculateActivityLevel()
        {
            return Math.Log(_sparks, ACTIVITY_LEVEL_LOG_BASE);
        }
    }
}
