using UnityEngine;

[System.Serializable]
public class RoomTypeEntry
{
    [SerializeField] public RoomType roomType;
    [SerializeField] public RoomGroupingEntry[] roomGroupingsEntry;
    [SerializeField][Range(0, 255)] public byte TypeWeight;
    [SerializeField][Range(0, 255)] public byte TypeLimit;

}