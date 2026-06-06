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
}