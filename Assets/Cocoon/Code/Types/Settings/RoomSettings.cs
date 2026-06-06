using UnityEngine;
[System.Serializable]
public class RoomSettings
{
    [Tooltip("The prefab used for the start room. The Enter node will be used as a spawn point for the object with the player tag.")]
    [SerializeField] private GameObject[] startRoomPrefabs;
    [SerializeField] private GameObject[] endRoomPrefabs;
    [SerializeField] private RoomTypeEntry[] randomRoomEntries;
    [SerializeField] private RequiredRoomEntry[] requiredRoomEntries;

    public GameObject[] getStartRoomPrefabs()
    {
        return startRoomPrefabs;
    }
    public GameObject[] getEndRoomPrefabs()
    {
        return endRoomPrefabs;
    }
    public RoomTypeEntry[] getRandomRoomEntries()
    {
        return randomRoomEntries;
    }
    public RequiredRoomEntry[] getRequiredRoomEntries()
    {
        return requiredRoomEntries;
    }
    public RoomSettings(GameObject[] startPrefabs, GameObject[] endPrefabs, RoomTypeEntry[] randomEntries, RequiredRoomEntry[] requiredEntries)
    {
        startRoomPrefabs = startPrefabs;
        endRoomPrefabs = endPrefabs;
        randomRoomEntries = randomEntries;
        requiredRoomEntries = requiredEntries;
    }
}