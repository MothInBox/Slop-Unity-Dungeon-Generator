using UnityEngine;

[System.Serializable]
public class RoomPrefabsEntry
{
    [Tooltip("The prefab for this room. Ensure the prefab has an Enter node and an Exit node for stitching with the other rooms.")]
    [SerializeField] private GameObject roomPrefab;
    [Tooltip("The weight of this room type. Higher weights increase the chance of this room being selected.")]
    [SerializeField][Range(0, 255)] private byte roomWeight;
    [Tooltip("The maximum number of times this room type can be placed.")]
    [SerializeField][Range(0, 255)] private byte roomLimit;

    public byte getWeight()
    {
        return roomWeight;
    }
    public byte getLimit()
    {
        return roomLimit;
    }
    public GameObject getPrefab()
    {
        return roomPrefab;
    }
    public RoomPrefabsEntry(GameObject prefab, byte weight, byte limit)
    {
        roomPrefab = prefab;
        roomWeight = weight;
        roomLimit = limit;
    }
}