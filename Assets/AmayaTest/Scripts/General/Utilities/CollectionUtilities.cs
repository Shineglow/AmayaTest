using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AmayaTest.Scripts.General.Utilities
{
    public static class CollectionUtilities
    {
        public static List<List<T>> InitializeCollectionWithSize<T>(int width, int height)
        {
            List<List<T>> result = new List<List<T>>(height);
            for (int i = 0; i < height; i++)
            {
                result.Add(new List<T>(width));
                for (int y = 0; y < width; y++)
                {
                    result[i].Add(default);
                }
            }

            return result;
        }

        public static T GetRandomItem<T>(this ICollection<T> collection)
        {
            return collection.ElementAt(Random.Range(0, collection.Count));
        }

        public static T GetRandomRemovedItem<T>(this ICollection<T> collection)
        {
            T result = collection.GetRandomItem();
            collection.Remove(result);
            return result;
        }
    }
}