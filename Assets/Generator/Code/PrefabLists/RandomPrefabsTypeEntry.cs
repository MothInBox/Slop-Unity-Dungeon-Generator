using UnityEngine;
[System.Serializable]
public class RandomPrefabsTypeEntry
{
    public RoomType roomType;
    [Range(0, 100)] public int weight = 50;
    public RoomWeights[] prefabs;
}