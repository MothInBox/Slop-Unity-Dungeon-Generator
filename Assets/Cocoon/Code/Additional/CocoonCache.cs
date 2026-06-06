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
        }
        else
        {
            Debug.LogWarning("Cache with ID " + cacheID + " already exists. Skipping creation.");
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
            }
            else
            {
                Debug.LogError("Cache with ID " + cacheID + " exists but has a different key/value type. Cannot add item.");
            }
        }
        else
        {
            Debug.LogError("Cache with ID " + cacheID + " does not exist. Cannot add item.");
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
                    return dictObj[key];
                }
                else
                {
                    Debug.LogWarning("Key " + key + " not found in cache with ID " + cacheID + ".");
                    return default(U);
                }
            }
            else
            {
                Debug.LogError("Cache with ID " + cacheID + " exists but has a different key/value type. Cannot retrieve item.");
                return default(U);
            }
        }
        else
        {
            Debug.LogError("Cache with ID " + cacheID + " does not exist. Cannot retrieve item.");
            return default(U);
        }
    }

    public void clearCache()
    {
        cacheCollection.Clear();
    }


}