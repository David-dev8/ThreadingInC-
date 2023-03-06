using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class DatabaseSimulationElementFactory : ISimulationElementFactory
    {
        public Animal CreateAnimal(Ecosystem ecosystem)
        {
            throw new NotImplementedException();
        }

        public Obstruction CreateObstruction(Ecosystem ecosystem)
        {
            throw new NotImplementedException();
        }

        public SimulationElement CreateRandomElement(Ecosystem ecosystem)
        {
            throw new NotImplementedException();
        }

        public Vegetation CreateVegetation(Ecosystem ecosystem)
        {
            throw new NotImplementedException();
        }
    }
}
