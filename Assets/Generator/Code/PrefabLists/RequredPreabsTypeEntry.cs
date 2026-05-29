using UnityEngine;
[System.Serializable]
public class RequiredPrefabsTypeEntry
{
    [Header("Prefab Settings")]
    [Tooltip("Type of room to be placed")]
    public RoomType roomType;
    [Tooltip("Prefabs to be used for this type)")]
    public GameObject[] prefabs;
    [Header("Depth Settings")]
    [Tooltip("Minimum depth for this room to be placed.")]

    public int depthMin = 0;
    [Tooltip("Maximum depth for this room to be placed.")]
    public int depthMax = 10;

    [Header("Count Settings")]
    [Tooltip("Minimum number of this room type to be placed.")]
    public int countMin = 1;
    [Tooltip("Maximum number of this room type to be placed.")]
    public int countMax = 1;
    [Tooltip("Number of this room type that have been placed.")]
    public int countreached = 0;
    [Tooltip("Current weight for placing this room type. This will move in a linear curve 0-100 depending on depth. Does not need to be touched. But ill leave it ope for debugging.")]
    public int currentWeight = 0;
    [Range(0, 100)] [Tooltip("Chance to place this room type after meeting the minimum count.")]
    public int afterMinChance = 1;

}

