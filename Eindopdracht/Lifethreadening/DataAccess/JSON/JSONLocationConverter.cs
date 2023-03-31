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
    /// <summary>
    /// This class is used to convert locations in a game into a json saveable format
    /// </summary>
    public class JSONLocationConverter : JsonConverter<Location>
    {
        private IDictionary<Location, string> _knownLocations = new Dictionary<Location, string>();

        private IDictionary<Location, IList<string>> _knownNeighbours = new Dictionary<Location, IList<string>>();
        private IDictionary<string, Location> _knownLocationsReversed = new Dictionary<string, Location>();

        /// <summary>
        /// Retrieves a location from a saved JSON worldstate
        /// </summary>
        /// <param name="reader">The reader to read the file</param>
        /// <param name="typeToConvert">The unused converter</param>
        /// <param name="options">The options</param>
        /// <returns>A location object of the given location</returns>
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

        /// <summary>
        /// Sews All the locations together So that all locations have all neighbors
        /// </summary>
        /// <param name="locations">All the locations to complete</param>
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

        /// <summary>
        /// Writes a location to a JSON file
        /// </summary>
        /// <param name="writer">The writer to use</param>
        /// <param name="value">The location to write</param>
        /// <param name="options">The options</param>
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

        /// <summary>
        /// Retrieves the ID of a location
        /// </summary>
        /// <param name="location">The Location to retrieve the ID for</param>
        /// <returns>The ID of the location</returns>
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
