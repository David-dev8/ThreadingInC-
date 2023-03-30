using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess
{
    public interface IEcosystemReader
    {
        IEnumerable<Ecosystem> ReadAll();
    }
}
