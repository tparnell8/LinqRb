﻿using System.Collections.Generic;

namespace System.Linq
{
    public static class IEnumerable
    {
        /// <summary>
        /// Return Enumerable where action is not true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<T> Reject<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            foreach(T element in source)
            {
                if(!predicate(element)) yield return element;
            }
        }

        /// <summary>
        /// Break a list of items into chunks of a specific size
        /// </summary>
        /// <param name="source"></param>
        /// <param name="chunkSize">How big you wish your chunks to be</param>
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunkSize)
        {
            var enumerator = source.GetEnumerator();
            var arr = new List<T>(chunkSize);
            while(enumerator.MoveNext())
            {
                arr.Add(enumerator.Current);
                if(arr.Count >= chunkSize)
                {
                    yield return arr;
                    arr = new List<T>(chunkSize);
                }
            }
            yield return arr;
        }

        /// <summary>
        /// Returns the first array that contains the object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IEnumerable<T> AssocFirstOrDefault<T>(this IEnumerable<IEnumerable<T>> source, T obj)
        {
            var outer = source.GetEnumerator();
            while(outer.MoveNext())
            {
                var inner = outer.Current.GetEnumerator();
                while(inner.MoveNext())
                {
                    if(inner.Current.Equals(obj))
                    {
                        return outer.Current;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Remove all nulls
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<T> Compact<T>(this IEnumerable<T> source)
            where T : class
        {
            foreach(var item in source)
            {
                if(item != null)
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Run action over array x times
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <param name="times">Times we should enumerate over the array</param>
        public static void Cycle<T>(this IEnumerable<T> source, Action<T> action, int times)
        {
            for(int i = 0; i < times; i++)
            {
                Enumerate(source, action);
            }
        }

        /// <summary>
        /// Infinate loop over Enumerable calling the action over each one
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void Cycle<T>(this IEnumerable<T> source, Action<T> action)
        {
            while(true)
            {
                Enumerate(source, action);
            }
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            return Enumerate(source, action);
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            var index = 0;
            var enumerator = source.GetEnumerator();
            while(enumerator.MoveNext())
            {
                action?.Invoke(enumerator.Current, index);
                yield return enumerator.Current;
                index++;
            }
        }

        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> source, Func<T, object> comparer)
        {
            var items = new HashSet<object>();
            foreach(var item in source)
            {
                var comparedItem = comparer?.Invoke(item);
                if(!items.Contains(comparedItem))
                {
                    items.Add(comparedItem);
                    yield return item;
                }
            }
        }

        private static IEnumerable<T> Enumerate<T>(IEnumerable<T> source, Action<T> action)
        {
            var enumerator = source.GetEnumerator();
            while(enumerator.MoveNext())
            {
                action?.Invoke(enumerator.Current);
                yield return enumerator.Current;
            }
        }
    }
}