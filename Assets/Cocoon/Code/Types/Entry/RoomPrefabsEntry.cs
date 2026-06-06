using UnityEngine;

[System.Serializable]
public class RoomPrefabsEntry
{
    [SerializeField] public GameObject roomPrefab;
    [SerializeField][Range(0, 255)] public byte RoomWeight;
    [SerializeField][Range(0, 255)] public byte RoomLimit;
}