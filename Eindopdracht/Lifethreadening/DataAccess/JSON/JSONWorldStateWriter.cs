using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Windows.Storage;

namespace Lifethreadening.DataAccess.JSON
{
    public class JSONWorldStateWriter : IWorldStateWriter
    {
        private const string SAVE_SLOT_FOLDER = "Games";
        private const string FILE_EXTENSION = ".json";

        public async Task<string> Write(string gameName, World world)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new LocationConverter());

            StorageFolder gameFolder = await GetGameFolder();
            StorageFile gameFile = await gameFolder.CreateFileAsync(gameName + FILE_EXTENSION, CreationCollisionOption.ReplaceExisting);

            using(Stream stream = await gameFile.OpenStreamForWriteAsync())
            {
                await JsonSerializer.SerializeAsync(stream, world, options);
            }
            // TODO is dit goed?
            return gameFile.Name;
        }

        public async Task Delete(string gameName)
        {
            StorageFolder gameFolder = await GetGameFolder();
            StorageFile gameFile = await gameFolder.GetFileAsync(gameName + FILE_EXTENSION);
            await gameFile.DeleteAsync(StorageDeleteOption.PermanentDelete);
        }

        private async Task<StorageFolder> GetGameFolder()
        {
            StorageFolder root = ApplicationData.Current.LocalFolder;
            StorageFolder gameFolder = await root.CreateFolderAsync(SAVE_SLOT_FOLDER, CreationCollisionOption.OpenIfExists);
            return gameFolder;
        }
    }

    public class LocationConverter : JsonConverter<Location>
    {
        private IDictionary<Location, string> _knownLocations = new Dictionary<Location, string>();

        public override Location Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Location value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteStartArray();
            foreach(Location location in value.Neighbours)
            {
                if(!_knownLocations.ContainsKey(location))
                {
                    _knownLocations.Add(location, _knownLocations.Count.ToString());
                }
                writer.WriteStringValue(_knownLocations[location]);
            }
            writer.WriteEndArray();

            JsonSerializer.Serialize(writer, value.SimulationElements, options);

            writer.WriteEndObject();
        }
    }
}
