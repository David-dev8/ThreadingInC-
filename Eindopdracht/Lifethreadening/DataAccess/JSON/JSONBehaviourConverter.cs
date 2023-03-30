using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Lifethreadening.Models.Behaviours;

namespace Lifethreadening.DataAccess.JSON
{
    public class JSONBehaviourConverter : JsonConverter<Behaviour>
    {
        private enum TypeDiscriminator
        {
            BreedBehaviour = 0,
            CarnivoreEatBehaviour = 1,
            CompositeBehavior = 2,
            CuriousWanderBehaviour = 3,
            EatBehaviour = 4,
            HerbivoreEatBehaviour = 5,
            HerdBehaviour = 6,
            PanicWanderBehaviour = 7,
            RestBehaviour = 8,
            RunAwayBehaviour = 9,
            WanderBehaviour = 10,
        }

        public override bool CanConvert(Type type)
        {
            return typeof(Behaviour).IsAssignableFrom(type);
        }

        public override Behaviour Read(
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

            Behaviour baseClass;
            TypeDiscriminator typeDiscriminator = (TypeDiscriminator)reader.GetInt32();
            switch(typeDiscriminator)
            {
                case TypeDiscriminator.CarnivoreEatBehaviour:
                    if(!reader.Read() || reader.GetString() != "TypeValue")
                    {
                        throw new JsonException();
                    }
                    if(!reader.Read() || reader.TokenType != JsonTokenType.StartObject)
                    {
                        throw new JsonException();
                    }
                    baseClass = (CarnivoreEatBehaviour)JsonSerializer.Deserialize(ref reader, typeof(CarnivoreEatBehaviour));
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
            Behaviour value,
            JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            if(value is CarnivoreEatBehaviour derivedA)
            {
                writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.CarnivoreEatBehaviour);
                writer.WritePropertyName("TypeValue");
                JsonSerializer.Serialize(writer, derivedA);
            }
            else
            {
                //throw new NotSupportedException();
            }

            writer.WriteEndObject();
        }
    }
}
