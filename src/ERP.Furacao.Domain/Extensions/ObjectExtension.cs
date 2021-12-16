using ERP.Furacao.Domain.Util;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text;

namespace ERP.Furacao.Domain.Extensions
{
    public static class ObjectExtension
    {
        /// <summary>
        /// Converte para o tipo genérico especificado.
        /// </summary>
        /// <typeparam name="T">Tipo genérico</typeparam>
        /// <param name="owner">Objeto extendido</param>
        /// <returns>Tipo convertido genérico</returns>
        public static T To<T>(this Object owner)
        {
            return TryConvert<T>(owner);
        }

        /// <summary>
        /// Passa um tipo para ser convertido para object.  
        /// </summary>
        /// <param name="owner">Objeto a ser extendido</param>
        /// <param name="typeTarget">Tipo genérico</param>
        /// <returns>Retorna verdadeiro ou falso</returns>
        public static Boolean CanConvertFrom(this Object owner, Type typeTarget)
        {
            try
            {
                return owner.TryConvert(typeTarget) != null;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Faz a conversão do tipo genérico para object e verifica a igualdade.
        /// </summary>
        /// <typeparam name="T">Tipo genérico</typeparam>
        /// <param name="owner">Objeto a ser extendido.</param>
        /// <returns>Retorna verdadeiro ou falso</returns>
        public static Boolean CanConvertFrom<T>(this Object owner)
        {
            try
            {
                return owner.IsDefaultValue<T>() || !owner.TryConvert<T>().Equals(default(T));
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Faz conversão do tipo genérico para object.
        /// </summary>
        /// <typeparam name="T">Tipo genérico</typeparam>
        /// <param name="owner">Objeto a ser extendido</param>
        /// <returns>Tipo genérico</returns>
        public static T TryConvert<T>(this Object owner)
        {
            try
            {
                return (T)Convert.ChangeType(owner, typeof(T));
            }
            catch (Exception)
            {
                try
                {
                    return (T)TypeUtil.ChangeType(owner, typeof(T));
                }
                catch (Exception)
                {
                    return default(T);
                }
            }
        }

        /// <summary>
        /// Faz a conversão de um tipo passado como argumento para o tipo object.
        /// </summary>
        /// <param name="owner">Objeto a ser extendido</param>
        /// <param name="typeTarget">Tipo a ser convertido</param>
        /// <returns>Um object</returns>
        public static Object TryConvert(this Object owner, Type typeTarget)
        {
            try
            {
                return Convert.ChangeType(owner, typeTarget);
            }
            catch (Exception)
            {
                try
                {
                    return TypeUtil.ChangeType(owner, typeTarget);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Converte para Boolean. 
        /// </summary>
        /// <param name="owner">Objeto a ser extendido</param>
        /// <returns>Retorna verdadeiro ou falso</returns>
        public static Boolean ToBoolean(this Object owner)
        {
            return owner.TryConvert<Boolean>();
        }

        /// <summary>
        /// Converte o objeto via reflexão para o tipo genérico.
        /// </summary>
        /// <typeparam name="T">Tipo genérico</typeparam>
        /// <param name="obj">Objeto a ser extendido</param>
        /// <returns>Um tipo genérico</returns>
        public static T ConvertTo<T>(this Object obj)
        {
            return new ReferenceTypeConverter().ConvertTo<T>(obj);
        }

        /// <summary>
        /// Converte o objeto via reflexão para o tipo genérico baseado em um layout.
        /// </summary>
        /// <typeparam name="T">Tipo genérico</typeparam>
        /// <param name="obj">Objeto extendido</param>
        /// <param name="typeConversionLayout">Tipo customizado de layout</param>
        /// <returns>Tipo genérico</returns>
        public static T ConvertTo<T>(this Object obj, TypeConversionLayoutUtil typeConversionLayout)
        {
            return new ReferenceTypeConverter().ConvertTo<T>(obj, typeConversionLayout);
        }

        /// <summary>
        /// Verifica o valor padrão.
        /// </summary>
        /// <param name="owner">Objeto a ser extendido</param>
        /// <returns>Verdadeiro ou falso</returns>
        public static Boolean IsDefaultValue(this Object owner, Type targetType = null)
        {
            if (owner == null)
                return true;

            targetType = targetType ?? owner.GetType();
            var defaultValue = targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
            return owner.Equals(defaultValue) || TypeUtil.ChangeType(owner, targetType).Equals(defaultValue);
        }

        /// <summary>
        /// Verifica o valor padrão.
        /// </summary>
        /// <param name="owner">Objeto a ser extendido</param>
        /// <returns>Verdadeiro ou falso</returns>
        public static Boolean IsDefaultValue<T>(this Object owner)
        {
            return IsDefaultValue(owner, typeof(T));
        }

        public static Boolean Is<T>(this T owner, Func<T, Boolean> condition)
        {
            return condition(owner);
        }

        public static Boolean IsTypeOf<T>(this Object owner)
        {
            return owner.GetType() == typeof(T);
        }

        public static String DisplayValues(this Object owner)
        {
            var sb = new StringBuilder();

            var properties = TypeDescriptor.GetProperties(owner).Sort(new String[] { "Name" });
            foreach (PropertyDescriptor property in properties)
            {
                //if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(String))
                sb.AppendLine("{0} : {1}".Fmt(property.Name, property.GetValue(owner)));
                //else if (property.)
                //{

                //}

                //else sb.AppendLine(property.GetValue(owner).DisplayValues());
            }

            return sb.ToString();
        }

        public static void ConsoleDisplayValue(this Object owner)
        {
            Console.WriteLine(owner);
        }

        public static void ConsoleDisplayValues(this Object owner)
        {
            Console.WriteLine(owner.DisplayValues());
        }

        public static Boolean IsNull(this Object owner)
        {
            return owner == null;
        }

        public static Boolean IsNotNull(this Object owner)
        {
            return !owner.IsNull();
        }

        //public static dynamic ToDynamic(this object value)
        //{
        //    IDictionary<string, object> expando = new ExpandoObject();

        //    foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
        //        expando.Add(property.Name, property.GetValue(value));

        //    return expando as ExpandoObject;
        //}       

        public static T InitializeProperties<T>(this T owner)
        {
            owner.GetType().GetProperties().ExecuteForEach(p =>
            {

                if (p.PropertyType.Name.ToLower() == "string")
                    p.SetValue(owner, String.Empty, null);

                if ((p.PropertyType.Name.ToLower() == ("int")) || (p.PropertyType.Name.ToLower() == ("int32")))
                    p.SetValue(owner, 0, null);

                if (p.PropertyType.Name.ToLower() == "decimal")
                    p.SetValue(owner, (decimal)0, null);

                if (p.PropertyType.Name.ToLower() == "dateTime")
                    p.SetValue(owner, DateTime.Today, null);
            });

            return owner;
        }

        public static T FillNullWithEmpty<T>(this T owner)
        {
            owner.GetType().GetProperties().ExecuteForEach(p =>
            {

                if (p.PropertyType.Name.ToLower() == "string")
                {
                    if (p.GetValue(owner) == null)
                        p.SetValue(owner, String.Empty, null);
                }

                if ((p.PropertyType.Name.ToLower() == ("int")) || (p.PropertyType.Name.ToLower() == ("int32")))
                {
                    if (p.GetValue(owner) == null)
                        p.SetValue(owner, 0, null);
                }

                if (p.PropertyType.Name.ToLower() == "decimal")
                {
                    if (p.GetValue(owner) == null)
                        p.SetValue(owner, (decimal)0, null);
                }
            });

            return owner;
        }

        public static R DefaultIfNull<T, R>(this T owner, Expression<Func<T, R>> expression, R returnDefault)
        {
            var retorno = returnDefault;

            if (owner.IsNotNull())
            {
                var value = expression.Compile().Invoke(owner);
                retorno = value == null ? retorno : value;
            }

            return retorno;
        }
    }
}
