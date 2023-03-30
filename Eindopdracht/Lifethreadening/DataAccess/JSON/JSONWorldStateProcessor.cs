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
    /// <summary>
    /// This class is used to process data used in saving and retrieving worldstates with json
    /// </summary>
    public abstract class JSONWorldStateProcessor
    {
        private const string SAVE_SLOT_FOLDER = "Games";
        private const string FILE_EXTENSION = "json";

        /// <summary>
        /// Retrieves the storage folder where saved worldstates are stored
        /// </summary>
        /// <returns></returns>
        protected async Task<StorageFolder> GetGameFolder()
        {
            StorageFolder root = ApplicationData.Current.LocalFolder;
            StorageFolder gameFolder = await root.CreateFolderAsync(SAVE_SLOT_FOLDER, CreationCollisionOption.OpenIfExists);
            return gameFolder;
        }

        /// <summary>
        /// Turns a game name into a file name by suffixing the corect extension
        /// </summary>
        /// <param name="gameName">The name of the game to get the filename from</param>
        /// <returns>The name of the file assosiated with the game</returns>
        protected string GetFileName(string gameName)
        {
            return gameName + "." + FILE_EXTENSION;
        }
    }
}
