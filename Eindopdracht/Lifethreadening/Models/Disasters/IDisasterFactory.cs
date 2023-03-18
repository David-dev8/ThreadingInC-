using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Disasters
{
    public interface IDisasterFactory
    {
        Disaster GetDisaster();
    }
}
