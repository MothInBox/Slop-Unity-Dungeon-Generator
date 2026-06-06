using UnityEngine;

[System.Serializable]
public class Exit : MonoBehaviour
{
    [SerializeField] private Gate[] placeIfBlocked;
    [SerializeField] private Gate[] placeIfNotBlocked;

    [SerializeField] private RoomType[] validRoomTypes;
    [SerializeField] private EntryType[] validEntryTypes;
    private bool isBlocked;
    private bool isConnected;
    private Entry myEntryNode;

    public void setBlocked(bool blocked)
    {
        isBlocked = blocked;
    }
    public Gate[] getGates()
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
    public RoomType[] getValidRoomTypes()
    {
        return validRoomTypes;
    }
    public bool IsBlocked()
    {
        return isBlocked;
    }
    public EntryType[] getValidEntryTypes()
    {
        return validEntryTypes;
    }

    private void OnDrawGizmos()
    {
        if (!CocoonLogger.doDrawGizmos()){return;}
        //Red if not connected, blue if connected
        Gizmos.color = isConnected ? Color.blue : Color.red;
        Gizmos.DrawSphere(transform.position, 0.25f);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2f);
    }
    public bool IsConnected()
    {
        return isConnected;
    }
    public void Connect(Entry entryNode)
    {
        if (entryNode.GetEntryType() == EntryType.Spawn)
        {
            CocoonLogger.LogWarning("Attempting to connect to a spawn entry. This should not happen. Ignoring.", 2, "Exit", "Connection");
            return;
        }
        if (isConnected){return;}
        myEntryNode = entryNode;
        myEntryNode.Connect(this);
        isConnected = true;
    }
    public void Disconnect()
    {
        if(!isConnected){return;}
        myEntryNode.Disconnect();
        isConnected = false;
    }
}