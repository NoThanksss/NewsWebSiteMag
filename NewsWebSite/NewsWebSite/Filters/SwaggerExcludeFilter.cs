using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using NewsWebSite_BLL.Attributes;

namespace NewsWebSite.Filters
{
    public class SwaggerExcludeFilter : ISchemaFilter
    {
        #region ISchemaFilter Members
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null)
            {
                return;
            }

            var ignoreDataMemberProperties = context.Type.GetProperties()
                .Where(t => t.GetCustomAttribute<SwaggerExcludeAttribute>() != null);

            foreach (var ignoreDataMemberProperty in ignoreDataMemberProperties)
            {
                var propertyToHide = schema.Properties.Keys
                    .SingleOrDefault(x => x.ToLower() == ignoreDataMemberProperty.Name.ToLower());

                if (propertyToHide != null)
                {
                    schema.Properties.Remove(propertyToHide);
                }
            }
        }

        #endregion
    }
}
