using System.Collections.Generic;
using UnityEngine;

namespace GameUtils {

    public static class ListUtils {

        #region Methods

        public static bool CheckEquals<T> (IReadOnlyList<T> list1, IReadOnlyList<T> list2) {
            // Check dictionary content count.
            if (list1.Count != list2.Count)
                return false;

            int itemCount = list1.Count;
            for (int i = 0; i < itemCount; i++) {
                // Check equality of same indexes.
                if (!list1[i].Equals (list2[i]))
                    return false;
            }

            return true;
        }

        public static IReadOnlyList<T> Shuffle<T> (IEnumerable<T> originalList) {
            List<T> shuffledList = new ();

            List<T> randomPool = new ();
            randomPool.AddRange (originalList);

            int itemCount = randomPool.Count;
            int randIndex = 0;
            for (int j = 0; j < itemCount; j++) {
                int currentCount = randomPool.Count;
                randIndex = (randIndex + Random.Range (0, currentCount)) % currentCount;
                shuffledList.Add (randomPool[randIndex]);
                randomPool.RemoveAt (randIndex);
            }

            return shuffledList;
        }

        #endregion

    }

}