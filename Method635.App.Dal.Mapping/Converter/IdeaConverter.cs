using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;

namespace Method635.App.Dal.Mapping.DTO
{
    public class BaseSpecifiedConcreteClassConverter : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (typeof(IdeaDto).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            return base.ResolveContractConverter(objectType);
        }
    }
    public class IdeaConverter : JsonConverter
    {
        private const string NoteIdeaTypeString = "noteIdea";
        private const string SketchIdeaTypeString = "sketchIdea";
        private const string PatternIdeaTypeString = "patternIdea";
        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new BaseSpecifiedConcreteClassConverter() };

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IdeaDto));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            if (jo.GetValue("type") == null)
            {
                return JsonConvert.DeserializeObject<NoteIdeaDto>(jo.ToString(), SpecifiedSubclassConversion);
            }
            switch (jo["type"].Value<string>())
            {
                case NoteIdeaTypeString:
                    return JsonConvert.DeserializeObject<NoteIdeaDto>(jo.ToString(), SpecifiedSubclassConversion);
                case PatternIdeaTypeString:
                    return JsonConvert.DeserializeObject<PatternIdeaDto>(jo.ToString(), SpecifiedSubclassConversion);
                case SketchIdeaTypeString:
                    return JsonConvert.DeserializeObject<SketchIdeaDto>(jo.ToString(), SpecifiedSubclassConversion);
                case "":
                    return JsonConvert.DeserializeObject<NoteIdeaDto>(jo.ToString(), SpecifiedSubclassConversion);
                default:
                    throw new Exception($"Unknown type '{jo["type"].Value<string>()}'");
            }
            throw new NotImplementedException();
        }
        public override bool CanWrite => true;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(value);

            if (t.Type != JTokenType.Object)
            {
                t.WriteTo(writer);
            }
            else
            {
                JObject o = (JObject)t;
                if (value is NoteIdeaDto)
                {
                    o.Add("type", JToken.FromObject(NoteIdeaTypeString));
                }
                else if (value is SketchIdeaDto)
                {
                    o.Add("type", JToken.FromObject(SketchIdeaTypeString));
                }
                else if (value is PatternIdeaDto)
                {
                    o.Add("type", JToken.FromObject(PatternIdeaTypeString));
                }

                o.WriteTo(writer);
            }
        }
    }
}
