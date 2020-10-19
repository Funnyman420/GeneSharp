using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeneSharp.Extensions
{
    public static class SystemExtension
    {

        /// <summary>
        /// Extension to overload the NextDouble method of Random.
        /// We want an easy and clean way to pick a random double between two doubles
        /// </summary>
        /// <param name="source">The Random object</param>
        /// <param name="min">The smallest value of value range</param>
        /// <param name="max">The highest value of value range</param>
        /// <returns>A double between <paramref name="min"/> and <paramref name="max"/></returns>
        public static double NextDouble(this Random source, double min, double max)
        {
            return source.NextDouble() * (max - min) + min;
        }

        /// <summary>
        /// Extension function that swaps two elements of an Enumerable at 
        /// the indexes of <paramref name="firstIndex"/> and <paramref name="secondIndex"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The IEnumerable object itself</param>
        /// <param name="firstIndex">The desired index to be swapped with <paramref name="secondIndex"/></param>
        /// <param name="secondIndex">The desired idnex to be swapped with <paramref name="firstIndex"/></param>
        /// <returns></returns>
        public static IEnumerable<T> Swap<T>(
            this IEnumerable<T> source,
            int firstIndex,
            int secondIndex)
        {
            var smallIndex = firstIndex < secondIndex ? firstIndex : secondIndex;
            var largeIndex = smallIndex == firstIndex ? secondIndex : firstIndex;

            using (IEnumerator<T> e = source.GetEnumerator())
            {
                for (var i = 0; i < smallIndex; i++)
                {
                    if (!e.MoveNext())
                        yield break;
                    yield return e.Current;
                }

                if (smallIndex != largeIndex)
                {
                    if (!e.MoveNext())
                        yield break;

                    var rememberedItem = e.Current;

                    var subset = new List<T>(largeIndex - smallIndex - 1);

                    for (int i = smallIndex + 1; i < largeIndex; i++)
                    {
                        if (!e.MoveNext())
                            break;
                        subset.Add(e.Current);
                    }

                    if (e.MoveNext())
                        yield return e.Current;

                    foreach (var item in subset)
                        yield return item;

                    yield return rememberedItem;
                }

                while (e.MoveNext())
                    yield return e.Current;
            }
        }
    }
}
