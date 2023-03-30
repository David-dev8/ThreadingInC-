using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess.JSON
{
    public class JSONLocationConverter : JsonConverter<Location>
    {
        private IDictionary<Location, string> _knownLocations = new Dictionary<Location, string>();

        private IDictionary<Location, IList<string>> _knownNeighbours = new Dictionary<Location, IList<string>>();
        private IDictionary<string, Location> _knownLocationsReversed = new Dictionary<string, Location>();

        public override Location Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonSerializerOptions newOptions = new JsonSerializerOptions();
            newOptions.Converters.Add(new JSONSimulationElementConverter());
            JSONLocation jsonLocation = JsonSerializer.Deserialize<JSONLocation>(ref reader, newOptions);
            Location location = new Location(jsonLocation.SimulationElements.ToList());
            _knownLocationsReversed.Add(jsonLocation.Id, location);
            _knownNeighbours.Add(location, jsonLocation.Neighbours);

            foreach(SimulationElement simulationElement in location.SimulationElements)
            {
                simulationElement.Location = location;
            }
            return location;
        }

        public void CompleteMapping(IEnumerable<Location> locations)
        {
            foreach(Location location in locations)
            {
                IList<Location> neighbours = new List<Location>();
                foreach(string neighbour in _knownNeighbours[location])
                {
                    neighbours.Add(_knownLocationsReversed[neighbour]);
                }
                location.Neighbours = neighbours;
            }
        }

        public override void Write(Utf8JsonWriter writer, Location value, JsonSerializerOptions options)
        {
            JsonSerializerOptions newOptions = new JsonSerializerOptions();
            newOptions.Converters.Add(new JSONSimulationElementConverter());
            IList<string> neighbours = new List<string>();
            foreach(Location location in value.Neighbours)
            {
                neighbours.Add(GetId(location));
            }

            JsonSerializer.Serialize(writer, new JSONLocation()
            {
                Id = GetId(value),
                Neighbours = neighbours,
                SimulationElements = value.SimulationElements
            }, newOptions);
        }

        private string GetId(Location location)
        {
            if(!_knownLocations.ContainsKey(location))
            {
                _knownLocations.Add(location, _knownLocations.Count.ToString());
            }
            return _knownLocations[location];
        }
    }
}
