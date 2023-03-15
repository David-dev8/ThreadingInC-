using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public interface ISimulationElementFactory
    {
        SimulationElement CreateRandomElement(Ecosystem ecosystem);
        Animal CreateAnimal(Ecosystem ecosystem);
        Vegetation CreateVegetation(Ecosystem ecosystem);
        Obstruction CreateObstruction(Ecosystem ecosystem);
    }
}
