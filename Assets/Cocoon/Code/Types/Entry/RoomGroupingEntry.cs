using UnityEngine;

[System.Serializable]
public class RoomGroupingEntry : IWeighted
{
    [Tooltip("The prefabs belonging to this grouping. The GroupWeight is calculated by combining the weights of all prefabs in this grouping. Ensure all grouped prefabs have the same Collision size and take into account the orientation after being placed (Entry node placement).")]
    [SerializeField] private RoomPrefabsEntry[] roomPrefabsEntry;
    [Tooltip("The maximum number of rooms that can be placed from this grouping.")]
    [SerializeField][Range(0, 255)] private byte groupLimit;



    private int groupWeight; // Get combined weights of all room prefabs in this grouping. Should not be set manually. 

    private void calculateWeight()
    {
        if (groupWeight != 0) return; // If groupWeight has already been calculated, skip the calculation.
        groupWeight = 0;
        foreach (RoomPrefabsEntry entry in roomPrefabsEntry)
        {
            groupWeight += entry.getWeight();
        }
    }
    public int getWeight()
    {
        calculateWeight();
        CocoonLogger.LogInfo("RoomGroupingEntry GroupWeight: " + groupWeight, 5, "RoomGroupingEntry", "Weight");
        return groupWeight;
    }
    public byte getLimit()
    {
        return groupLimit;
    }
    public RoomPrefabsEntry[] getRoomPrefabsEntry()
    {
        return roomPrefabsEntry;
    }
    public RoomGroupingEntry(RoomPrefabsEntry[] prefabsEntry, byte limit)
    {
        roomPrefabsEntry = prefabsEntry;
        groupLimit = limit;
    }
}
