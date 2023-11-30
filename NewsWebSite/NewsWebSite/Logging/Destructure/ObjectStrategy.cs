using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NewsWebSite.Logging.Destructure
{
    public class ObjectStrategy : ISensitiveDataStrategy
    {
        private readonly List<ISensitiveDataStrategy> _strategies;

        public ObjectStrategy(RemoveFromLoggingStrategy removeFromLoggingStrategy)
        {
            _strategies = new List<ISensitiveDataStrategy>();
            _strategies.Add(new SerializableStrategy());
            _strategies.Add(removeFromLoggingStrategy);
            _strategies.Add(new ArrayStrategy(removeFromLoggingStrategy));
        }

        public bool IsApplicable(object value)
        {
            var valueInfo = value.GetType().GetTypeInfo();
            return !valueInfo.IsSerializable;
        }

        public Dictionary<string, object> Apply(Dictionary<string, object> dictionary, object value)
        {
            var validEntries = value
                .GetType()
                .GetTypeInfo()
                .DeclaredProperties
                .Where(p => p.GetValue(value) != null)
                .ToList();

            var innerDictionary = ApplyInternal(validEntries, value);

            string objName = value.GetType().GetTypeInfo().Name;
            objName = dictionary.ContainsKey(objName) ? Guid.NewGuid().ToString() : objName;

            return new Dictionary<string, object>() { { objName, innerDictionary } };
        }

        public Dictionary<string, object> Apply(PropertyInfo propertyInfo, Dictionary<string, object> dictionary, object value)
        {
            var validEntries = value
                .GetType()
                .GetTypeInfo()
                .DeclaredProperties
                .Where(p => p.GetValue(value) != null)
                .ToList();

            var innerDictionary = ApplyInternal(validEntries, value);

            string objName = propertyInfo.Name;
            objName = dictionary.ContainsKey(objName) ? Guid.NewGuid().ToString() : objName;

            return new Dictionary<string, object>() { { objName, innerDictionary } };
        }

        private Dictionary<string, object> ApplyInternal(IEnumerable<PropertyInfo> validEntries, object value)
        {
            var propertyValueDictionary = new Dictionary<string, object>();

            foreach (var propertyInfo in validEntries)
            {
                var propValue = propertyInfo.GetValue(value);
                var result = _strategies.First(strategy => strategy.IsApplicable(propValue)).Apply(propertyInfo, propertyValueDictionary, propValue);
                result.ToList().ForEach(x => propertyValueDictionary.Add(x.Key, x.Value));
            }

            return propertyValueDictionary;
        }
    }
}
