using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine.UI;
using Random = UnityEngine.Random;


namespace Utility
{
    public static class Utils
    {
        public static int GetRandomWithExclusion(int start, int end, int[] exclude)
        {
            //var range = Enumerable.Range(start, end-1).Where(i => !exclude.Contains(i));
            List<int> range = new List<int>();
            for (int i = start; i < end; i++)
            {
                if (exclude.Contains(i))
                {
                    continue;
                }
                range.Add(i);
            }
            int index = Random.Range(0,range.Count);
            return range[index];
        }


        public static T ChangeAlpha<T>(this T g, float newAlpha)
            where T : Graphic
        {
            var color = g.color;
            color.a = newAlpha;
            g.color = color;
            return g;
        }
    }
    public static class EnumerableExtension
    {
        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }
    }

    public static class RandomUtils
    {
        
    }
    
}