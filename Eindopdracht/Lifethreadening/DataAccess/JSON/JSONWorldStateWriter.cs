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
    /// <summary>
    /// This class is used to save a Worldstate to a JSON file
    /// </summary>
    public class JSONWorldStateWriter : JSONWorldStateProcessor, IWorldStateWriter
    {
        public async Task<string> Write(string gameName, World world)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new JSONWorldConverter());
            options.Converters.Add(new JSONLocationConverter());
            options.Converters.Add(new JSONSimulationElementConverter());

            StorageFolder gameFolder = await GetGameFolder();
            StorageFile gameFile = await gameFolder.CreateFileAsync(GetFileName(gameName), CreationCollisionOption.ReplaceExisting);

            using(Stream stream = await gameFile.OpenStreamForWriteAsync())
            {
                await JsonSerializer.SerializeAsync(stream, world, options);
            }
            return gameFile.Name;
        }

        public async Task Delete(string gameName)
        {
            StorageFolder gameFolder = await GetGameFolder();
            StorageFile gameFile = await gameFolder.GetFileAsync(GetFileName(gameName));
            await gameFile.DeleteAsync(StorageDeleteOption.PermanentDelete);
        }
    }
}
