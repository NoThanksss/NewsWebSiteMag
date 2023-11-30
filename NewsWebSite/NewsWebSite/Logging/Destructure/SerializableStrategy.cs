using NewsWebSite_BLL.Attributes;
using System.Reflection;

namespace NewsWebSite.Logging.Destructure
{
    public class SerializableStrategy : ISensitiveDataStrategy
    {
        public SerializableStrategy()
        {
        }

        public bool IsApplicable(object value)
        {
            var valueInfo = value.GetType().GetTypeInfo();
            return valueInfo.IsSerializable && !(valueInfo.IsConstructedGenericType || valueInfo.IsArray);
        }

        public Dictionary<string, object> Apply(Dictionary<string, object> dictionary, object value)
        {
            return new Dictionary<string, object>() { { Guid.NewGuid().ToString(), value } };
        }

        public Dictionary<string, object> Apply(PropertyInfo propertyInfo, Dictionary<string, object> dictionary, object value)
        {
            var attr = propertyInfo.GetCustomAttribute<JsonPropertyWithSensitiveDataAttribute>();
            var dictionaryValue = attr != null ? "*****" : value;
            var dictionaryKey = dictionary.ContainsKey(propertyInfo.Name) ? Guid.NewGuid().ToString() : propertyInfo.Name;

            return new Dictionary<string, object>() { { dictionaryKey, dictionaryValue } };
        }
    }
}
