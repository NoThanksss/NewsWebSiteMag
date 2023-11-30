using System.Reflection;
using NewsWebSite_BLL.Attributes;

namespace NewsWebSite.Logging.Destructure
{
    public class RemoveFromLoggingStrategy : ISensitiveDataStrategy
    {
        private readonly ObjectStrategy _objectStrategy;

        private TypeInfo valueInfo;

        public RemoveFromLoggingStrategy()
        {
            _objectStrategy = new ObjectStrategy(this);
        }

        public bool IsApplicable(object value)
        {
            valueInfo = value.GetType().GetTypeInfo();
            return !valueInfo.IsSerializable;
        }

        public Dictionary<string, object> Apply(Dictionary<string, object> dictionary, object value)
        {
            var attr = valueInfo.GetCustomAttribute<NotLoggingAttribute>();

            if (attr != null)
            {
                string objName = value.GetType().GetTypeInfo().Name;
                objName = dictionary.ContainsKey(objName) ? Guid.NewGuid().ToString() : objName;

                return new Dictionary<string, object>() { { objName, "Not logged." } };
            }
            else
            {
                return _objectStrategy.Apply(dictionary, value);
            }
        }

        public Dictionary<string, object> Apply(PropertyInfo propertyInfo, Dictionary<string, object> dictionary, object value)
        {
            var attr = valueInfo.GetCustomAttribute<NotLoggingAttribute>();

            if (attr != null)
            {
                string objName = propertyInfo.Name;
                objName = dictionary.ContainsKey(objName) ? Guid.NewGuid().ToString() : objName;

                return new Dictionary<string, object>() { { objName, "Not logged." } };
            }
            else
            {
                return _objectStrategy.Apply(propertyInfo, dictionary, value);
            }
        }
    }
}
