using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace ERP.Furacao.Domain.Extensions
{
    public static class LambdaExtension
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> knownKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static MemberInfo GetMember(this LambdaExpression expression)
        {
            if (expression.Body is MemberExpression)
                return (expression.Body as MemberExpression).Member;
            else if (expression.Body is UnaryExpression)
                return ((expression.Body as UnaryExpression).Operand as MemberExpression).Member;
            return null;
        }

        public static String GetMemberName(this LambdaExpression expression)
        {
            if (expression.Body is MemberExpression)
                return (expression.Body as MemberExpression).Member.Name;
            else if (expression.Body is UnaryExpression)
                return ((expression.Body as UnaryExpression).Operand as MemberExpression).Member.Name;
            return String.Empty;
        }

        public static MemberInfo GetMemberFromExpression<T>(this Expression<Func<T, object>> expression)
        {
            if (expression.Body.NodeType != ExpressionType.MemberAccess)
            {
                return ((MemberExpression)((UnaryExpression)expression.Body).Operand).Member;
            }
            return ((MemberExpression)expression.Body).Member;
        }
    }
}
