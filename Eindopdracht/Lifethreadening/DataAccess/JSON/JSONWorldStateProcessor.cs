using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace Lifethreadening.DataAccess.JSON
{
    public abstract class JSONWorldStateProcessor
    {
        private const string SAVE_SLOT_FOLDER = "Games";
        private const string FILE_EXTENSION = "json";

        protected async Task<StorageFolder> GetGameFolder()
        {
            StorageFolder root = ApplicationData.Current.LocalFolder;
            StorageFolder gameFolder = await root.CreateFolderAsync(SAVE_SLOT_FOLDER, CreationCollisionOption.OpenIfExists);
            return gameFolder;
        }

        protected string GetFileName(string gameName)
        {
            return !gameName.EndsWith(FILE_EXTENSION) ? (gameName + "." + FILE_EXTENSION) : gameName;
        }
    }
}
