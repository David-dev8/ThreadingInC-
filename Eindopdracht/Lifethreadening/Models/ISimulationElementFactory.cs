using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public interface ISimulationElementFactory
    {
        SimulationElement CreateRandomElement(WorldContextService contextService);
        Animal CreateAnimal(WorldContextService contextService);
        Vegetation CreateVegetation(WorldContextService contextService);
        Obstruction CreateObstruction(WorldContextService contextService);
    }
}
