using UnityEngine;
using System;

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

    public static int Randomize(long seed)
    {
        if (seed == 0)
        {
            seed = RandomizeSeed(seed);
            random = new System.Random((int)(seed & 0xFFFFFFFF) ^ (int)(seed >> 32));
        }
        return random.Next();
    }

    public static void placeRoom(GameObject roomPrefab, Vector3 position, Quaternion rotation)
    {
        GameObject.Instantiate(roomPrefab, position, rotation);
    }
    public static void DestroyRoom(GameObject room)
    {
        GameObject.Destroy(room);
    }
}