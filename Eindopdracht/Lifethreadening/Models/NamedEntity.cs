using System;
using Lifethreadening.Base;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    // TODO elke klasse constructor bekijken
    /// <summary>
    /// This class is used to store data about names
    /// </summary>
    public class NamedEntity : Observable // TODO elke klasse met een name property moet hiervan overerven
    {
        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }

    }
}
