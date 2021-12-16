using ERP.Furacao.Domain.Extensions;
using System;

namespace ERP.Furacao.Domain.Util
{
    public class ReferenceTypeConverter
    {
        /// <summary>
        /// Converte um argumento do tipo object para um tipo genérico.
        /// </summary>
        /// <typeparam name="T">Tipo genérico</typeparam>
        /// <param name="obj">Obejeto a ser convertido</param>
        /// <returns>Um tipo genérico convertido</returns>
        public T ConvertTo<T>(Object obj)
        {
            T result = Activator.CreateInstance<T>();

            try
            {
                foreach (var property in obj.GetType().GetProperties())
                {
                    try
                    {
                        result.GetType().GetProperty(property.Name).SetValue(result, property.GetValue(obj, null).TryConvert(result.GetType().GetProperty(property.Name).PropertyType), null);
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }

            return result;
        }

        /// <summary>
        /// Sobrecarga do método "ConvertTo". Converte um argumento do tipo object, através de um tipo layout passado como argumento, para um tipo genérico.
        /// </summary>
        /// <typeparam name="T">Tipo genérico</typeparam>
        /// <param name="obj">Objeto a ser convertido</param>
        /// <param name="typeConversionLayout">Layout para conversão</param>
        /// <returns>Um tipo genérico convertido</returns>
        public T ConvertTo<T>(Object obj, TypeConversionLayoutUtil typeConversionLayout)
        {
            T result = Activator.CreateInstance<T>();

            try
            {
                foreach (TypeConversionLayoutItemUtil layoutItem in typeConversionLayout)
                {
                    try
                    {
                        result.GetType().GetProperty(layoutItem.TargetName).SetValue(result, obj.GetType().GetProperty(layoutItem.SourceName).GetValue(obj, null).TryConvert(result.GetType().GetProperty(layoutItem.TargetName).PropertyType), null);
                    }
                    catch
                    {
                    }
                }
            }
            catch { }
            return result;
        }
    }
}
