using ERP.Furacao.Domain.Extensions;
using System;
using System.ComponentModel;

namespace ERP.Furacao.Domain.Util
{
    public class TypeUtil
    {
        /// <summary>
        /// Retorna um System.Object com o System.Type especificado e cujo valor é
        /// equivalente ao objeto especificado.
        /// </summary>
        /// <param name="value">Objeto a ser convertido</param>
        /// <param name="conversionType">Tipo do objeto que precisa ser convertido</param>
        /// <returns>Um Objeto convertido</returns>
        public static Object ChangeType(Object value, Type conversionType)
        {
            if (conversionType == null)
                throw new ArgumentNullException("conversionType não informado");

            if (conversionType.IsGenericType &&
                conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                    return null;

                NullableConverter nullableConverter = new NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }

            if (
                    value != null &&
                    conversionType == typeof(DateTime) &&
                    value.ToString().IsNullOrEmpty()
                )
                return null;

            if (conversionType.IsEnum)
                try
                {
                    value = Enum.Parse(conversionType, value.ToString());
                }
                catch
                {
                    value = null;
                }

            var typeConverter = TypeDescriptor.GetConverter(conversionType);
            if (typeConverter != null)
            {
                if (typeConverter.CanConvertFrom(value.GetType()))
                    return typeConverter.ConvertFrom(value);
            }


            return Convert.ChangeType(value, conversionType);
        }
    }
}
