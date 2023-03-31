using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess.API.GeneDTOs
{
    /// <summary>
    /// This class is used to store protiens
    /// </summary>
    public class Protein
    {
        [JsonPropertyName("recommendedName")]
        public ProteinName Name { get; set; }
    }
}
