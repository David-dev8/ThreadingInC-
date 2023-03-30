using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using System.Reflection.PortableExecutable;

namespace Lifethreadening.DataAccess.JSON
{
    public class JSONSimulationElementConverter : JsonConverter<SimulationElement>
    {
        private enum TypeDiscriminator
        {
            Vegetation = 0,
            Animal = 1,
            Obstruction = 2
        }

        public override bool CanConvert(Type type)
        {
            return typeof(SimulationElement).IsAssignableFrom(type);
        }

        public override SimulationElement Read(
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

            SimulationElement baseClass;
            TypeDiscriminator typeDiscriminator = (TypeDiscriminator)reader.GetInt32();
            switch(typeDiscriminator)
            {
                case TypeDiscriminator.Vegetation:
                    if(!reader.Read() || reader.GetString() != "TypeValue")
                    {
                        throw new JsonException();
                    }
                    if(!reader.Read() || reader.TokenType != JsonTokenType.StartObject)
                    {
                        throw new JsonException();
                    }
                    baseClass = (Vegetation)JsonSerializer.Deserialize(ref reader, typeof(Vegetation));
                    break;
                case TypeDiscriminator.Animal:
                    if(!reader.Read() || reader.GetString() != "TypeValue")
                    {
                        throw new JsonException();
                    }
                    if(!reader.Read() || reader.TokenType != JsonTokenType.StartObject)
                    {
                        throw new JsonException();
                    }
                    JsonSerializerOptions newOptions = new JsonSerializerOptions();
                    newOptions.Converters.Add(new JSONBehaviourConverter());
                    baseClass = (Animal)JsonSerializer.Deserialize(ref reader, typeof(Animal), newOptions);
                    break;
                case TypeDiscriminator.Obstruction:
                    if(!reader.Read() || reader.GetString() != "TypeValue")
                    {
                        throw new JsonException();
                    }
                    if(!reader.Read() || reader.TokenType != JsonTokenType.StartObject)
                    {
                        throw new JsonException();
                    }
                    baseClass = (Obstruction)JsonSerializer.Deserialize(ref reader, typeof(Obstruction));
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
            SimulationElement value,
            JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            if(value is Obstruction derivedA)
            {
                writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.Obstruction);
                writer.WritePropertyName("TypeValue");
                JsonSerializer.Serialize(writer, derivedA);
            }
            else if(value is Vegetation derivedB)
            {
                writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.Vegetation);
                writer.WritePropertyName("TypeValue");
                JsonSerializer.Serialize(writer, derivedB);
            }
            else if(value is Animal derivedC)
            {
                JsonSerializerOptions newOptions = new JsonSerializerOptions();
                newOptions.Converters.Add(new JSONBehaviourConverter());
                writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.Animal);
                writer.WritePropertyName("TypeValue");
                JsonSerializer.Serialize(writer, derivedC, newOptions);
            }
            else
            {
                throw new NotSupportedException();
            }

            writer.WriteEndObject();
        }
    }
}
