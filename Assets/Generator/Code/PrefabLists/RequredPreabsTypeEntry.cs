using UnityEngine;
[System.Serializable]
public class RequiredPrefabsTypeEntry
{
    [Header("Prefab Settings")]
    public RoomType roomType;
    public GameObject[] prefabs;
    [Header("Depth Settings")]
    public int depthMin = 0;
    public int depthMax = 10;
    [Header("Count Settings")]
    public int countMin = 1;
    public int countMax = 1;
    public int countreached = 0;
    public int currentWeight = 0;

}

