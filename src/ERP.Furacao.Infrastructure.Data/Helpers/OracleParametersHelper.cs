using Dapper.Oracle;
using ERP.Furacao.Domain.Attributes;
using System.Linq;

namespace ERP.Furacao.Infrastructure.Data.Helpers
{
    public static class OracleParametersHelper
    {
        public static object Entity { get; set; }
        public static OracleDynamicParameters Parameters
        {
            get
            {
                var parameters = new OracleDynamicParameters();

                var props = Entity.GetType().GetProperties();

                foreach (var prop in props)
                {
                    var attribute = (OracleParameterAttribute)prop.GetCustomAttributes(typeof(OracleParameterAttribute), true).FirstOrDefault();
                    var valor = GetPropertyValue<object>(Entity, prop.Name);
                    parameters.Add(attribute.Name, valor, attribute.Type, attribute.Direction, size: attribute.Size);
                }

                return parameters;
            }
        }

        public static T GetPropertyValue<T>(object o, string propertyName)
        {
            return (T)o.GetType().GetProperty(propertyName).GetValue(o, null);
        }

    }

}
