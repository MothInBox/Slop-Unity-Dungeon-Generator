using UnityEngine;

[System.Serializable]
public class Exit : MonoBehaviour
{
    [SerializeField] private Gate[] placeIfBlocked;
    [SerializeField] private Gate[] placeIfNotBlocked;

    [SerializeField] private RoomType[] validRoomTypes;
    [SerializeField] private EntryType[] allowedEntryTypes;
    private bool isBlocked;

    public void SetBlocked(bool blocked)
    {
        isBlocked = blocked;
    }
    public Gate[] GetGates()
    {
        if (isBlocked)
        {
            return placeIfBlocked;
        }
        else
        {
            return placeIfNotBlocked;
        }
    }
    public RoomType[] GetValidRoomTypes()
    {
        return validRoomTypes;
    }
    public bool IsBlocked()
    {
        return isBlocked;
    }
    public EntryType[] GetAllowedEntryTypes()
    {
        return allowedEntryTypes;
    }
}