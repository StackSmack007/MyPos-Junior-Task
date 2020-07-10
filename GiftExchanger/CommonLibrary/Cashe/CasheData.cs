using System.Collections.Generic;

namespace CommonLibrary.Cashe
{
    public static class CasheData
    {
        private static IDictionary<string, object> cashe;
        static CasheData()
        {
            cashe = new Dictionary<string, object>();
        }

        public static bool HasData(string name) =>
            cashe.ContainsKey(name) && cashe[name] != null;

        public static void ResetData(string name) =>
            cashe[name] = null;

        public static void Save<T>(string name, T item)
            where T : class
        {
            cashe[name] = item;
        }

        public static T Retrieve<T>(string name)
            where T : class
        {
            return cashe[name] as T;
        }
    }
}