using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public interface IBreedFactory
    {
        IEnumerable<Animal> CreateAnimals(Animal father, Animal mother, WorldContextService contextService);
    }
}
