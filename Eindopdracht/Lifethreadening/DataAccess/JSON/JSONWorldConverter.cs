using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Lifethreadening.Models;

namespace Lifethreadening.DataAccess.JSON
{
    public class JSONWorldConverter : JsonConverter<World>
    {
        private enum TypeDiscriminator
        {
            World = 0,
            GridWorld = 1
        }

        public override bool CanConvert(Type type)
        {
            return typeof(World).IsAssignableFrom(type);
        }

        public override World Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if(reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            if(!reader.Read()
                    || reader.TokenType != JsonTokenType.PropertyName
                    || reader.GetString() != "TypeDiscriminator")
            {
                throw new JsonException();
            }

            if(!reader.Read() || reader.TokenType != JsonTokenType.Number)
            {
                throw new JsonException();
            }

            World baseClass;
            TypeDiscriminator typeDiscriminator = (TypeDiscriminator)reader.GetInt32();
            switch(typeDiscriminator)
            {
                case TypeDiscriminator.GridWorld:
                    if(!reader.Read() || reader.GetString() != "TypeValue")
                    {
                        throw new JsonException();
                    }
                    if(!reader.Read() || reader.TokenType != JsonTokenType.StartObject)
                    {
                        throw new JsonException();
                    }
                    JsonSerializerOptions newOptions = new JsonSerializerOptions();
                    newOptions.Converters.Add(options.Converters.Where(c => c is JSONLocationConverter).First());
                    baseClass = (GridWorld)JsonSerializer.Deserialize(ref reader, typeof(GridWorld), newOptions);
                    break;
                default:
                    throw new NotSupportedException();
            }

            if(!reader.Read() || reader.TokenType != JsonTokenType.EndObject)
            {
                throw new JsonException();
            }

            return baseClass;
        }

        public override void Write(
            Utf8JsonWriter writer,
            World value,
            JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            if(value is GridWorld derivedA)
            {
                writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.GridWorld);
                writer.WritePropertyName("TypeValue");
                JsonSerializerOptions newOptions = new JsonSerializerOptions();
                newOptions.Converters.Add(new JSONLocationConverter());
                JsonSerializer.Serialize(writer, derivedA, newOptions);
            }
            else
            {
                throw new NotSupportedException();
            }

            writer.WriteEndObject();
        }
    }
}
