using UnityEngine;

[System.Serializable]
public enum RoomType
{
    BASIC, // A standard room with no special properties.
    LARGE, // A larger room that may contain more enemies or loot.
    SMALL, // A smaller room that may be easier to navigate but contain fewer rewards.
}