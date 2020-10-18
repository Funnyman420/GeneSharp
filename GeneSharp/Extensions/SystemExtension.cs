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
        /// Implementation for Deep Cloning Objects. First we convert
        /// them to a Json Serialized Object and then we desirialize it
        /// to the given Type
        /// </summary>
        /// <typeparam name="T">The type of the Object that we want to clone</typeparam>
        /// <param name="source">The Object that we want to clone</param>
        /// <returns>The values of the object that we want to clone packed in a new object</returns>
        public static double NextDouble(this Random source, double min, double max)
        {
            return source.NextDouble() * (max - min) + min;
        }

        public static IEnumerable<T> Swap<T> (this IEnumerable<T> source, 
            int firstIndex, 
            int secondIndex)
        {
            using (IEnumerator<T> e = source.GetEnumerator())
            {
                for (var i = 0; i < firstIndex; i++)
                {
                    if (!e.MoveNext())
                        yield break;
                    yield return e.Current;
                }

                if (firstIndex != secondIndex)
                {
                    if (!e.MoveNext())
                        yield break;

                    var rememberedItem = e.Current;

                    var subset = new List<T>(secondIndex - firstIndex - 1);

                    for (int i = firstIndex + 1; i < secondIndex; i++)
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
