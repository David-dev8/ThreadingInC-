using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Ecosystem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Ecosystem(int id, string name)        {
            Id = id;
            Name = name;
        }
    }
}
