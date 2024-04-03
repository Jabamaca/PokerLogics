using System.Collections.Generic;

namespace GameUtils {

    public static class DictionaryUtils {

        #region Methods

        public static bool CheckEquals<K, V> (IReadOnlyDictionary<K, V> dict1, IReadOnlyDictionary<K, V> dict2) {
            // Check dictionary content count.
            if (dict1.Count != dict2.Count)
                return false;

            foreach (KeyValuePair<K, V> kvp1 in dict1) {

                // Try get value of common key.
                if (dict2.TryGetValue (kvp1.Key, out V value2)) {
                    V value1 = kvp1.Value;

                    // Check value equality.
                    if (!value1.Equals (value2))
                        return false;

                } else {
                    // Current key from Dictionary 1 not found in Dictionary 2.
                    return false;

                }
            }

            return true;
        }

        #endregion

    }

}