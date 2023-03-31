using Lifethreadening.DataAccess.Database;
using Lifethreadening.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lifethreadening.Helpers;
using Lifethreadening.ExtensionMethods;

namespace Lifethreadening.Models
{
    /// <summary>
    /// This class is used to create ofspring with genes from both parents equally
    /// </summary>
    public class EvenlyDistributedParentsBreedFactory : IBreedFactory
    {
        private const int WEIGHT_DEVIATION = 2;
        private const int SIZE_DEVIATION = 4;

        private Random _random = new Random();
        private readonly IBehaviourBuilder _behaviourBuilder;
        private readonly INameReader _nameReader;

        /// <summary>
        /// Creates a new evenly distributed parents breed factory
        /// </summary>
        /// <param name="behaviourBuilder">The builder to use for building behviours</param>
        /// <param name="nameReader">The reader to use for names</param>
        public EvenlyDistributedParentsBreedFactory(IBehaviourBuilder behaviourBuilder, INameReader nameReader)
        {
            _behaviourBuilder = behaviourBuilder;
            _nameReader = nameReader;
        }

        /// <summary>
        /// This function creates ofspring
        /// </summary>
        /// <param name="father">The first parent</param>
        /// <param name="mother">The second parent</param>
        /// <param name="contextService">The context service</param>
        /// <returns>A collection with all ofspring</returns>
        public IEnumerable<Animal> CreateAnimals(Animal father, Animal mother, WorldContextService contextService)
        {
            if(CanBreed(father, mother))
            {
                Species species = father.Species;
                int amountOfChildren = _random.Next(species.MinBreedSize, species.MaxBreedSize + 1);
                IList<Animal> children = new List<Animal>();
                for(int i = 0; i < amountOfChildren; i++)
                {
                    Animal animal = CreateAnimal(father, mother, contextService);
                    children.Add(animal);
                }
                return children;
            }
            return Enumerable.Empty<Animal>();
        }

        /// <summary>
        /// This function creates a single ofspring
        /// </summary>
        /// <param name="father">The first parent</param>
        /// <param name="mother">The second parent</param>
        /// <param name="contextService">The context service</param>
        /// <returns>The ofspring</returns>
        private Animal CreateAnimal(Animal father, Animal mother, WorldContextService contextService)
        {
            Species species = father.Species;
            Sex sex = EnumHelpers.GetRandom<Sex>();
            string name = _nameReader.GetName(sex);
            Animal newAnimal = new Animal(name, sex, species, MergeStatistics(species, father.Statistics, mother.Statistics), contextService);
            newAnimal.Behaviour = _behaviourBuilder
                .ForAnimal(newAnimal)
                .AddEat(species.Diet)
                .AddBreed(this)
                .AddWander(true, _random.NextDouble(mother.Age / species.MaxAge))
                .AddRest()
                .AddEvade()
                .GetBehaviour();
            return newAnimal;
        }

        /// <summary>
        /// This function merges the statistics of both parents 
        /// </summary>
        /// <param name="species">The spiecies of the parents</param>
        /// <param name="first">The first parents statistics</param>
        /// <param name="second">The second parents statistics</param>
        /// <returns>The merged statistics</returns>
        private Statistics MergeStatistics(Species species, Statistics first, Statistics second)
        {
            Statistics newStatistics = species.BaseStatistics.Clone();
            newStatistics.Weight = newStatistics.Weight.Deviate(WEIGHT_DEVIATION);
            newStatistics.Size = newStatistics.Size.Deviate(SIZE_DEVIATION);
            newStatistics.Speed = first.Speed.AverageWith(second.Speed);
            newStatistics.Aggresion = first.Aggresion.AverageWith(second.Aggresion);
            newStatistics.Detection = first.Detection.AverageWith(second.Detection);
            newStatistics.Resilience = first.Resilience.AverageWith(second.Resilience);
            newStatistics.Intelligence = first.Intelligence.AverageWith(second.Intelligence);
            newStatistics.SelfDefence = first.SelfDefence.AverageWith(second.SelfDefence);
            newStatistics.MetabolicRate = first.MetabolicRate.AverageWith(second.MetabolicRate);
            return newStatistics;
        }

        /// <summary>
        /// This function checks if two animals can breed
        /// </summary>
        /// <param name="father">The first parent</param>
        /// <param name="mother">The second parent</param>
        /// <returns>A boolean value indicating if the parent can breed</returns>
        private bool CanBreed(Animal father, Animal mother)
        {
            return father.Species.Equals(mother.Species) && 
                father.Sex == Sex.MALE  && mother.Sex == Sex.FEMALE;
        }
    }
}
