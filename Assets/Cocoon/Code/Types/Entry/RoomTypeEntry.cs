using UnityEngine;

[System.Serializable]
public class RoomTypeEntry : IWeighted
{
    [Tooltip("The type of room this entry represents. This can be used to categorize rooms and apply specific generation rules based on the type.")]
    [SerializeField] private RoomType roomType;
    [Tooltip("Combine multiple rooms with same collision into a grouping. Take into account the orientation after being placed (Entry node placement).")]
    [SerializeField] private RoomGroupingEntry[] roomGroupingsEntry;
    [Tooltip("The weight of this room type. Higher weights increase the chance of this room type being selected during generation.")]
    [SerializeField][Range(0, 255)] private byte typeWeight;
    [Tooltip("The maximum number of times this room type can be placed.")]
    [SerializeField][Range(0, 255)] private byte typeLimit;

    public RoomType getType()
    {
        return roomType;
    }
    public int getWeight()
    {
        return (int)typeWeight;
    }
    public byte getLimit()
    {
        return typeLimit;
    }
    public RoomGroupingEntry[] getRoomGroupingsEntry()
    {
        return roomGroupingsEntry;
    }
    public RoomTypeEntry(RoomType type, RoomGroupingEntry[] groupings, byte weight, byte limit)
    {
        roomType = type;
        roomGroupingsEntry = groupings;
        typeWeight = weight;
        typeLimit = limit;
    }

}