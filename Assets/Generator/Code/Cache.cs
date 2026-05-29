using UnityEngine;


using System.Collections.Generic;

public static class Cache
{
    private static Dictionary<RoomType, RandomPrefabsTypeEntry> roomTypeCache;
    private static Dictionary<GameObject, RoomWeights> roomWeightCache;
    private static HashSet<RoomType> warnedMissingTypes;
    private static HashSet<GameObject> warnedMissingRooms;

    public static void BuildCache(RandomPrefabsTypeEntry[] randomPrefabs)
    {

        roomTypeCache = new Dictionary<RoomType, RandomPrefabsTypeEntry>();
        roomWeightCache = new Dictionary<GameObject, RoomWeights>();
        warnedMissingTypes = new HashSet<RoomType>();
        warnedMissingRooms = new HashSet<GameObject>();
        foreach (RandomPrefabsTypeEntry entry in randomPrefabs)
        {
            roomTypeCache[entry.roomType] = entry;
            foreach (RoomWeights rw in entry.prefabs)
            {
                roomWeightCache[rw.prefab] = rw;
            }
        }
    }

    public static void ClearCache()
    {
        roomTypeCache = null;
        roomWeightCache = null;
        warnedMissingTypes = null;
        warnedMissingRooms = null;
    }

    public static RandomPrefabsTypeEntry GetEntryForType(RoomType type)
    {
        if (roomTypeCache.TryGetValue(type, out RandomPrefabsTypeEntry entry))
        {
            return entry;
        }
        else
        {
            if (warnedMissingTypes.Add(type))
            {
                DebugHolder.LogWarning($"Cache miss: room type '{type}' has no RandomPrefabsTypeEntry. Add it to Generator.randomPrefabs or remove it from exit allowed types.");
            }
            return null;
        }
    }
    public static RoomWeights GetEntryForRoom(GameObject room)
    {
        if (roomWeightCache.TryGetValue(room, out RoomWeights rw))
        {
            return rw;
        }
        else
        {
            if (warnedMissingRooms.Add(room))
            {
                DebugHolder.LogWarning($"Cache miss: prefab '{room.name}' has no RoomWeights entry. Add weight data under its room type in Generator.randomPrefabs.");
            }
            return null;
        }
    }

    public static int GetWeightForType(RoomType type)
    {
        if (roomTypeCache.TryGetValue(type, out RandomPrefabsTypeEntry entry))
        {
            return entry.weight;
        }
        else
        {
            if (warnedMissingTypes.Add(type))
            {
                DebugHolder.LogWarning($"Cache miss: room type '{type}' has no RandomPrefabsTypeEntry. Add it to Generator.randomPrefabs or remove it from exit allowed types.");
            }
            return 0;
        }
    }

    public static int GetWeightForRoom(GameObject room)
    {
        if (roomWeightCache.TryGetValue(room, out RoomWeights rw))
        {
            return rw.weight;
        }
        else
        {
            if (warnedMissingRooms.Add(room))
            {
                DebugHolder.LogWarning($"Cache miss: prefab '{room.name}' has no RoomWeights entry. Add weight data under its room type in Generator.randomPrefabs.");
            }
            return 0;
        }
    }
}

