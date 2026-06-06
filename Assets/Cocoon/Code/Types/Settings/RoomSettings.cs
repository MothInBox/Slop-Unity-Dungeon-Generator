using UnityEngine;
[System.Serializable]
public class RoomSettings
{
    [Tooltip("The prefab used for the start room. The Enter node will be used as a spawn point for the object with the player tag.")]
    [SerializeField] private GameObject startRoomPrefab;
    [SerializeField] private RoomTypeEntry[] randomRoomEntries;
}