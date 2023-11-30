using System.Collections;
using System.Reflection;

namespace NewsWebSite.Logging.Destructure
{
    public class ArrayStrategy : ISensitiveDataStrategy
    {
        private readonly List<ISensitiveDataStrategy> _strategies;

        public ArrayStrategy(RemoveFromLoggingStrategy removeFromLoggingStrategy)
        {
            _strategies = new List<ISensitiveDataStrategy>();
            _strategies.Add(new SerializableStrategy());
            _strategies.Add(removeFromLoggingStrategy);
            _strategies.Add(this);
        }

        public bool IsApplicable(object value)
        {
            var valueInfo = value.GetType().GetTypeInfo();
            return valueInfo.IsConstructedGenericType || valueInfo.IsArray;
        }

        public Dictionary<string, object> Apply(Dictionary<string, object> dictionary, object value)
        {
            var arrayDict = new Dictionary<string, object>();
            var keyName = "List";

            foreach (var element in (IEnumerable)value)
            {
                var result = _strategies.First(strategy => strategy.IsApplicable(element)).Apply(arrayDict, element);
                result.ToList().ForEach(x => arrayDict.Add(x.Key, x.Value));
            }

            keyName = dictionary.ContainsKey(keyName) ? Guid.NewGuid().ToString() : keyName;

            return new Dictionary<string, object>() { { "List", arrayDict.Values } };
        }

        public Dictionary<string, object> Apply(PropertyInfo propertyInfo, Dictionary<string, object> dictionary, object value)
        {
            var arrayDictionary = new Dictionary<string, object>();

            foreach (var element in (IEnumerable)value)
            {
                var result = _strategies.First(strategy => strategy.IsApplicable(element)).Apply(propertyInfo, arrayDictionary, element);
                result.ToList().ForEach(x => arrayDictionary.Add(x.Key, x.Value));
            }

            var keyName = dictionary.ContainsKey(propertyInfo.Name) ? Guid.NewGuid().ToString() : propertyInfo.Name;

            return new Dictionary<string, object>() { { propertyInfo.Name, arrayDictionary.Values } };
        }
    }
}
