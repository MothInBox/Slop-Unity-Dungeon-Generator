using UnityEngine;

[System.Serializable]
public class RoomGroupingEntry
{
    [SerializeField] public RoomPrefabsEntry[] roomPrefabsEntry;
    [SerializeField][Range(0, 255)] public byte GroupLimit;



    private int GroupWeight; // Get combined weights of all room prefabs in this grouping. Should not be set manually. 

    private void calculateGroupWeight()
    {
        if (GroupWeight != 0) return; // If GroupWeight has already been calculated, skip the calculation.
        GroupWeight = 0;
        foreach (RoomPrefabsEntry entry in roomPrefabsEntry)
        {
            GroupWeight += entry.RoomWeight;
        }
    }
    public int GetGroupWeight()
    {
        calculateGroupWeight();
        CocoonLogger.LogInfo("RoomGroupingEntry GroupWeight: " + GroupWeight);
        return GroupWeight;
    }
}
