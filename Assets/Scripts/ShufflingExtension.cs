using System;
using System.Collections.Generic;
using System.Linq;

namespace Map
{
    public static class ShufflingExtension
    {

        // not my code!!!!!
        // got it here: http://stackoverflow.com/questions/273313/randomize-a-listt/1262619#1262619 
        private static System.Random rng = new System.Random();

        public static void Shuffle<T>(this IList<T> list) //이 메서드는 IList<T>를 구현한 컬렉션을 섞는 역할
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static T Random<T>(this IList<T> list) // 리스트에서 임의의 요소를 선택하여 반환
        {
            return list[rng.Next(list.Count)];
        }

        public static T Last<T>(this IList<T> list) // 리스트에서 마지막 요소를 반환
        {
            return list[list.Count - 1];
        }

        public static List<T> GetRandomElements<T>(this List<T> list, int elementsCount) //리스트에서 지정된 수의 요소를 무작위로 선택하여 반환
        {
            return list.OrderBy(arg => Guid.NewGuid()).Take(list.Count < elementsCount ? list.Count : elementsCount)
                .ToList();
        }
    }
}
