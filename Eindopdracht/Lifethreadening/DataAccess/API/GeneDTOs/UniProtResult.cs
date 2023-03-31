using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess.API.GeneDTOs
{
    /// <summary>
    /// This class is used to store unique protiens
    /// </summary>
    /// <typeparam name="T">The type of protiens</typeparam>
    public class UniProtResult<T>
    {
        public IEnumerable<T> Results { get; set; }
    }
}
