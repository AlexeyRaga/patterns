using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Patterns.Infrastructure
{
    public static class JsonDocument
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

        public static string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value, Formatting.Indented, Settings);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }

        public static dynamic DeserializeToDynamic(string json)
        {
            return JsonConvert.DeserializeObject<dynamic>(json, Settings);
        }

        public static T CopyInto<T>(this DynamicObject value)
        {
            var json = Serialize(value);
            return Deserialize<T>(json);
        }
        
    }
}
