using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ERP.Furacao.Domain.Extensions
{
    public static class CollectionExtension
    {
        public static void ExecuteForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
                return;

            source.ToList<T>().ForEach(action);
        }

        public static void ExecuteForEach<T>(this IEnumerable<T> source, Action<T, Int32> action)
        {
            var index = 0;

            source.ExecuteForEach(x =>
            {
                action(x, index++);
            });
        }

        public static IEnumerable<T> ExecuteForEach<T>(this IEnumerable<T> source, Action<T> action, Func<T, Boolean> predicate)
        {
            source
                .Where(predicate)
                .ToList<T>()
                .ForEach(action);

            return source;
        }

        public static void RemoveItem<T>(this ICollection<T> owner, Func<T, Boolean> predicate)
        {
            owner.Remove(
                owner.Where(predicate).FirstOrDefault()
                );
        }

        public static void ChangeItem<T>(this IList<T> owner, Func<T, Boolean> predicate, T newItem)
        {
            var item = owner.Where(predicate).FirstOrDefault();

            if (item != null)
            {
                var index = owner.IndexOf(item);

                owner[index] = newItem;
            }
        }

        public static Boolean CollectionEquals<T>(this IEnumerable<T> owner, IEnumerable<T> other)
        {
            return Enumerable.SequenceEqual<T>(owner, other);
        }

        public static Type GetCollectionItemType<T>(this IEnumerable<T> owner)
        {
            return owner.GetType().GetGenericArguments()[0];
        }

        public static IEnumerable<T> FastReverse<T>(this IEnumerable<T> owner)
        {
            for (int i = owner.Count() - 1; i >= 0; i--)
            {
                yield return owner.ToList()[i];
            }
        }

        public static void SafeAdd<T>(this ICollection<T> owner, T item)
        {
            if (!owner.Contains(item))
                owner.Add(item);
        }

        public static IEnumerable<TSource> FromHierarchy<TSource>(
            this TSource source,
            Func<TSource, TSource> nextItem,
            Func<TSource, bool> canContinue)
        {
            for (var current = source; canContinue(current); current = nextItem(current))
            {
                yield return current;
            }
        }

        public static IEnumerable<TSource> FromHierarchy<TSource>(
            this TSource source,
            Func<TSource, TSource> nextItem)
            where TSource : class
        {
            return FromHierarchy(source, nextItem, s => s != null);
        }

        public static TSource SingleOrDefault<TSource>(this IQueryable<TSource> source, String errorMessage)
        {
            try
            {
                return source.SingleOrDefault();
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException(FormatErrorMessage(ex, errorMessage), ex.InnerException);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(FormatErrorMessage(ex, errorMessage), ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new Exception(FormatErrorMessage(ex, errorMessage));
            }
        }

        private static string FormatErrorMessage(Exception ex, string errorMessage)
        {
            if (errorMessage.IsNullOrEmpty())
                return ex.Message;
            else
                return "{0} {1}Erro Original: {2}".Fmt(errorMessage, System.Environment.NewLine, ex.Message);
        }

        public static TSource SingleOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, String errorMessage)
        {
            try
            {
                return source.SingleOrDefault(predicate);
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException(FormatErrorMessage(ex, errorMessage), ex.InnerException);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(FormatErrorMessage(ex, errorMessage), ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new Exception(FormatErrorMessage(ex, errorMessage));
            }
        }

        public static IEnumerable<IEnumerable<TSource>> Split<TSource>(this IEnumerable<TSource> source, int elements)
        {
            return source.Select((value, index) =>
            {
                return new { Index = index, Value = value };
            })
                .GroupBy(x => x.Index / elements)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
        }
    }
}
