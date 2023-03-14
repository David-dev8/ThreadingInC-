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
    public class EvenlyDistributedParentsBreedFactory : IBreedFactory
    {
        private const int WEIGHT_DEVIATION = 2;
        private const int SIZE_DEVIATION = 4;

        private Random _random = new Random();
        private readonly IBehaviourBuilder _behaviourBuilder;

        public EvenlyDistributedParentsBreedFactory(IBehaviourBuilder behaviourBuilder)
        {
            _behaviourBuilder = behaviourBuilder;
        }

        public IEnumerable<Animal> CreateAnimals(Animal father, Animal mother)
        {
            if(CanBreed(father, mother))
            {
                Species species = father.Species;
                int amountOfChildren = _random.Next(species.MinBreedSize, species.MaxBreedSize + 1);
                IList<Animal> children = new List<Animal>();
                for(int i = 0; i < amountOfChildren; i++)
                {
                    children.Add(CreateAnimal(father, mother));
                }
                return children;
            }
            return Enumerable.Empty<Animal>();
        }

        private Animal CreateAnimal(Animal father, Animal mother)
        {
            Species species = father.Species;
            Animal newAnimal = new Animal(EnumHelpers.GetRandom<Sex>(), species, MergeStatistics(species, father.Statistics, mother.Statistics));
            newAnimal.Behaviour = _behaviourBuilder
                .ForAnimal(newAnimal)
                .AddEat(species.Diet)
                .AddBreed(this) // TODO
                .AddWander(true, _random.NextDouble(mother.Age / species.MaxAge))
                .AddRest()
                .AddEvade()
                .GetBehaviour();
            return newAnimal;
        }

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

        private bool CanBreed(Animal father, Animal mother)
        {
            return father.Species.Equals(mother.Species) && 
                father.Sex == Sex.MALE  && mother.Sex == Sex.FEMALE;
        }
    }
}
