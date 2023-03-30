﻿using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Documents;
using System.Collections;
using Windows.UI.Xaml.Data;
using WinRTXamlToolkit.Input;

namespace Lifethreadening.DataAccess.JSON
{
    public class JSONWorldStateReader: JSONWorldStateProcessor, IWorldStateReader
    {
        public async Task<World> Read(string gameName)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            JSONLocationConverter locationConverter = new JSONLocationConverter();
            options.Converters.Add(locationConverter);
            options.Converters.Add(new JSONWorldConverter());

            StorageFolder gameFolder = await GetGameFolder();
            StorageFile gameFile = await gameFolder.GetFileAsync(GetFileName(gameName));

            World world;
            using(Stream stream = await gameFile.OpenStreamForReadAsync())
            {
                world = await JsonSerializer.DeserializeAsync<World>(stream, options);
                locationConverter.CompleteMapping(world.Locations);
            }
            foreach(SimulationElement element in world.SimulationElements)
            {
                element.ContextService = world.ContextService;
            }
            return world;
        }
    }
}
