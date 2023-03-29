using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess.Database
{
    public class DatabaseSimulationWriter : ISimulationWriter
    {
        public Task Write(string saveSlotLocation, Simulation simulation)
        {
            return Task.CompletedTask;
            //throw new NotImplementedException();
        }
    }
}
