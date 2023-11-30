using System.Collections.Generic;
using System.Reflection;

namespace NewsWebSite.Logging.Destructure
{
    public interface ISensitiveDataStrategy
    {
        public bool IsApplicable(object value);

        public Dictionary<string, object> Apply(Dictionary<string, object> dictionary, object value);

        public Dictionary<string, object> Apply(PropertyInfo propertyInfo, Dictionary<string, object> dictionary, object value);
    }
}
