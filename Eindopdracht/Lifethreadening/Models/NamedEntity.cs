using System;
using Lifethreadening.Base;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    // TODO elke klasse constructor bekijken
    public class NamedEntity : Observable // TODO elke klasse met een name property moet hiervan overerven
    {
        public string Name { get; set; }
    }
}
