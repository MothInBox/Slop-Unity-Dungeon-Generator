using UnityEngine;
using System.Collections.Generic;

public class CocoonCache
    {
        private Dictionary<string, object> cacheCollection = new Dictionary<string, object>();

        public void newCache<T, U>(string cacheID)
        {
            if (!cacheCollection.ContainsKey(cacheID))
            {
                cacheCollection[cacheID] = new Dictionary<T, U>();
                CocoonLogger.LogInfo("Created cache with ID: " + cacheID, 3, "CocoonCache", "Cache");
            }
            else
            {
                CocoonLogger.LogWarning("Cache with ID " + cacheID + " already exists. Skipping creation.", 2, "CocoonCache", "Cache");
            }
        }

        public void setInCache<T, U>(string cacheID, T key, U value)
        {
            if (cacheCollection.ContainsKey(cacheID))
            {
                var dictObj = cacheCollection[cacheID] as Dictionary<T, U>;
                if (dictObj != null)
                {
                    dictObj[key] = value;
                    CocoonLogger.LogInfo("Set cache item '" + key + "' in cache: " + cacheID, 4, "CocoonCache", "Cache");
                }
                else
                {
                    CocoonLogger.LogError("Cache with ID " + cacheID + " exists but has a different key/value type. Cannot add item.", 1, "CocoonCache", "Cache");
                }
            }
            else
            {
                CocoonLogger.LogError("Cache with ID " + cacheID + " does not exist. Cannot add item.", 1, "CocoonCache", "Cache");
            }
        }

        public U getFromCache<T, U>(string cacheID, T key)
        {
            if (cacheCollection.ContainsKey(cacheID))
            {
                var dictObj = cacheCollection[cacheID] as Dictionary<T, U>;
                if (dictObj != null)
                {
                    if (dictObj.ContainsKey(key))
                    {
                        U val = dictObj[key];
                        CocoonLogger.LogInfo("Retrieved cache item '" + key + "' from cache: " + cacheID, 4, "CocoonCache", "Cache");
                        return val;
                    }
                    else
                    {
                        CocoonLogger.LogWarning("Key " + key + " not found in cache with ID " + cacheID + ".", 2, "CocoonCache", "Cache");
                        return default(U);
                    }
                }
                else
                {
                    CocoonLogger.LogError("Cache with ID " + cacheID + " exists but has a different key/value type. Cannot retrieve item.", 1, "CocoonCache", "Cache");
                    return default(U);
                }
            }
            else
            {
                CocoonLogger.LogError("Cache with ID " + cacheID + " does not exist. Cannot retrieve item.", 1, "CocoonCache", "Cache");
                return default(U);
            }
        }

        public void removeCache(string cacheID)
        {
            if (cacheCollection.ContainsKey(cacheID))
            {
                cacheCollection.Remove(cacheID);
                CocoonLogger.LogInfo("Removed cache with ID: " + cacheID, 3, "CocoonCache", "Cache");
            }
            else
            {
                CocoonLogger.LogWarning("Cache with ID " + cacheID + " does not exist. Cannot remove.", 2, "CocoonCache", "Cache");
            }
        }

        public void removeFromCache<T, U>(string cacheID, T key)
        {
            if (cacheCollection.ContainsKey(cacheID))
            {
                var dictObj = cacheCollection[cacheID] as Dictionary<T, U>;
                if (dictObj != null)
                {
                    if (dictObj.ContainsKey(key))
                    {
                        dictObj.Remove(key);
                        CocoonLogger.LogInfo("Removed key '" + key + "' from cache: " + cacheID, 4, "CocoonCache", "Cache");
                    }
                    else
                    {
                        CocoonLogger.LogWarning("Key " + key + " not found in cache with ID " + cacheID + ". Cannot remove.", 2, "CocoonCache", "Cache");
                    }
                }
                else
                {
                    CocoonLogger.LogError("Cache with ID " + cacheID + " exists but has a different key/value type. Cannot remove item.", 1, "CocoonCache", "Cache");
                }
            }
            else
            {
                CocoonLogger.LogError("Cache with ID " + cacheID + " does not exist. Cannot remove item.", 1, "CocoonCache", "Cache");
            }
        }

        public void clearCache()
        {
            cacheCollection.Clear();
            CocoonLogger.LogInfo("Cleared all caches.", 3, "CocoonCache", "Cache");
        }

    }