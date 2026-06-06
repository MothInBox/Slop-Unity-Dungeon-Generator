using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections;

public static class CocoonUtility
{
    private static System.Random random = new System.Random();
    public static long RandomizeSeed(long seed)
    {
        if (seed == 0)
        {
            seed = DateTime.Now.Ticks;
        }
        //System.Random does not play nice with long types, so we combine two random integers instead.
        random = new System.Random((int)(seed & 0xFFFFFFFF) ^ (int)(seed >> 32)); //XOR two halves of seed
        return ((long)random.Next() << 32) | (long)random.Next(); // Combine two random integers to create a new long seed
    }

    public static Scene createScene(string sceneName)
    {
        return SceneManager.CreateScene(sceneName);
    }

    public static int Randomize(long seed)
    {
        if (seed == 0)
        {
            seed = RandomizeSeed(seed);
            random = new System.Random((int)(seed & 0xFFFFFFFF) ^ (int)(seed >> 32));
        }
        return random.Next();
    }

    public static Room placeRoom(GameObject roomPrefab, Vector3 position, Quaternion rotation)
    {
        return GameObject.Instantiate(roomPrefab, position, rotation).GetComponent<Room>();
    }
    public static void DestroyRoom(GameObject room)
    {
        GameObject.Destroy(room);
    }

    public static T GetRandomElement<T>(T[] array, long seed)
    {
        try {
            if (array == null || array.Length == 0)
            {
                throw new ArgumentException("Start Prefab Array is null or empty.");
            }
            return array[Randomize(seed) % array.Length];
        } catch (Exception ex)
        {
            CocoonLogger.LogException(ex, 1, "CocoonUtility", "Exception");
            return default(T);
        }
    }

    public static Queue AddExitsToQueue(Room room, Queue exitQueue)
     {
        foreach (Exit exit in room.getExits())
        {
            exitQueue.Enqueue(exit);
        }
        return exitQueue;
     }
     public static void ClearExitQueue(Queue exitQueue)
    {
        exitQueue.Clear();
    }

    public static T WeightedRandom<T>(string cacheKey, long seed, CocoonCache cache) where T : IWeighted
    {
        return default(T);
    }
    public static bool BuildCacheForSettings(string keyPrefix, CocoonCache cache, RoomSettings roomSettings)
    {  
        int typeCounter = 0;
        int groupCounter = 0; 
        try
        {
            if (cache == null)
            {
                throw new ArgumentNullException("cache", "CocoonCache instance is required to build settings cache.");
            }

            if (roomSettings == null)
            {
                throw new ArgumentNullException("roomSettings", "RoomSettings is required to build caches.");
            }

            RoomTypeEntry[] randomRoomEntries = roomSettings.getRandomRoomEntries();
            if (randomRoomEntries == null || randomRoomEntries.Length == 0)
            {
                throw new ArgumentException("RoomSettings has no random room entries to cache.");
            }

            string typesCacheId = keyPrefix + "_Types";
            string groupingsCacheId = keyPrefix + "_Groupings";

            // Build cache of RoomType to RoomTypeEntry and EntryType to RoomGroupingEntry.
            cache.newCache<RoomType, RoomTypeEntry>(typesCacheId);
            cache.newCache<EntryType, RoomGroupingEntry>(groupingsCacheId);

            foreach (RoomTypeEntry typeEntry in randomRoomEntries)
            {
                if (typeEntry == null)
                {
                    CocoonLogger.LogWarning("Encountered a null RoomTypeEntry while building cache. Skipping.", 2, "CocoonUtility", "Cache");
                    continue;
                }

                cache.setInCache<RoomType, RoomTypeEntry>(typesCacheId, typeEntry.getType(), typeEntry);
                typeCounter++;
                //Build cache of EntryType to RoomGroupingEntry for this type
                RoomGroupingEntry[] groupingEntries = typeEntry.getRoomGroupingsEntry();
                if (groupingEntries == null || groupingEntries.Length == 0)
                {
                    CocoonLogger.LogWarning("RoomTypeEntry " + typeEntry.getType() + " has no grouping entries. Skipping grouping cache population.", 3, "CocoonUtility", "Cache");
                    continue;
                }

                foreach (RoomGroupingEntry groupingEntry in groupingEntries)
                {
                    if (groupingEntry == null)
                    {
                        CocoonLogger.LogWarning("Encountered a null RoomGroupingEntry while building cache. Skipping.", 2, "CocoonUtility", "Cache");
                        continue;
                    }

                    cache.setInCache<EntryType, RoomGroupingEntry>(groupingsCacheId, groupingEntry.getEntryType(), groupingEntry);
                    groupCounter++;
                }
            }
            CocoonLogger.LogInfo("Cached: " + typeCounter + " types and " + groupCounter + " groups.", 4, "CocoonUtility", "Cache");
            return true;
        } catch (System.Exception ex)
        {
            ex.Data.Add("Cached: " + typeCounter + " types and " + groupCounter + " groups before failing.", "CacheBuildInfo");
            CocoonLogger.LogException(ex, 1, "CocoonUtility", "Exception");
            return false;
        }
    }
}