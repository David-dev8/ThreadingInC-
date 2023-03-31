using Lifethreadening.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Disasters
{
    /// <summary>
    /// This class is used to contain data about flooding disasters
    /// </summary>
    public class FloodingDisaster : Disaster
    {
        private const string DESCRIPTION = "Flooding";
        private const int MIN_TOTAL_WAVE_PUSHES = 5000;
        private const int MAX_TOTAL_WAVE_PUSHES = 1000000;
        private const int MAX_WAVE_STRENGH = 200000;
        private const int MIN_STRENGTH_TO_ADD = 1;
        private const int MAX_STRENGTH_TO_ADD = 13;
        private const int MINIMUM_ORDER_OF_MAGNITUDE_TO_DEAL_DAMAGE = 2;

        private Random _random = new Random();
        private int _totalPushes;

        /// <summary>
        /// Creates a new flooding disaster
        /// </summary>
        /// <param name="contextService">The contextservice</param>
        public FloodingDisaster(WorldContextService contextService) : base(DESCRIPTION, contextService)
        {
            _totalPushes = _random.Next(MIN_TOTAL_WAVE_PUSHES, MAX_TOTAL_WAVE_PUSHES);
        }

        public override string GetLongDescription()
        {
            return $"FloodingDisaster. Initated at: {DateInitiated:d}. Runoff flooding: {CalculateRunOff():0.00} cubic metre flow per hour.";
        }

        public override void Strike(IEnumerable<SimulationElement> simulationElements)
        {
            // Waves accumulate in strength
            // They are released
            // The higher the wave, the more chance for the flood gates to open
            int currentWaveStrength = 0;
            for(int i = 0; i < _totalPushes; i++)
            {
                // The push adds random strength to the current wave
                currentWaveStrength += _random.Next(MIN_STRENGTH_TO_ADD, MAX_STRENGTH_TO_ADD);
                // Every time, check whether the current wave is released
                // The chance of releasing a wave increases with the strength it accumulated
                double releaseChance = currentWaveStrength / (double)MAX_WAVE_STRENGH;
                if(_random.NextDouble() < releaseChance) 
                {
                    // Release and damage all animals based on the order of magnitude of the wave strength
                    int orderOfMagnitude = (int)Math.Log10(currentWaveStrength) - MINIMUM_ORDER_OF_MAGNITUDE_TO_DEAL_DAMAGE;
                    if(orderOfMagnitude > 0)
                    {
                        foreach(SimulationElement simulationElement in simulationElements)
                        {
                            Damage(simulationElement, orderOfMagnitude);
                        }
                    }
                    currentWaveStrength = 0;
                }
            }
        }

        /// <summary>
        /// This method damages game elements based on the given damage 
        /// </summary>
        /// <param name="simulationElement">A list of aplicable game elements</param>
        /// <param name="damage">The damage that the flooding causes</param>
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

        /// <summary>
        /// This method calculates how fast the flood is dissapating
        /// </summary>
        /// <returns>A value showing how much of the flood has dissapated</returns>
        private double CalculateRunOff()
        {
            return Math.Sqrt(_totalPushes);
        }
    }
}
